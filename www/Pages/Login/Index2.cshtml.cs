using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nucleo.Modelos;
using System.ComponentModel.DataAnnotations;
using www.Utilidades;

namespace www.Pages.Login;

public class Index2Model : PageModel
{
    private const int MaximoFallos = 3;
    private static readonly TimeSpan DuracionBloqueo = TimeSpan.FromMinutes(1);
    private static readonly object SincronizacionIntentos = new();
    private static readonly Dictionary<string, EstadoIntentos> IntentosPorEmail = new();

    private readonly IDatos _datos;

    public Index2Model(IDatos datos)
    {
        _datos = datos;
    }

    [BindProperty]
    [Required(ErrorMessage = "Debe indicar una contraseña.")]
    [DataType(DataType.Password)]
    public string Contraseña { get; set; } = "";

    public string EMail { get; set; } = "";

    public DateTimeOffset? BloqueoHasta { get; private set; }


    public IActionResult OnGet()
    {
        EMail = ObtenerEmailPendiente();

        if (string.IsNullOrEmpty(EMail))
        {
            return RedirectToPage("/Login/Index");
        }

        BloqueoHasta = ObtenerBloqueoHasta(EMail);
        return Page();
    }

    public IActionResult OnPostAtras()
    {
        HttpContext.Session.Remove(ClavesSesion.EmailPendiente);
        return RedirectToPage("/Login/Index");
    }

    public IActionResult OnPostAceptar()
    {
        EMail = ObtenerEmailPendiente();

        if (string.IsNullOrEmpty(EMail))
        {
            return RedirectToPage("/Login/Index");
        }

        BloqueoHasta = ObtenerBloqueoHasta(EMail);

        if (BloqueoHasta is not null)
        {
            ModelState.AddModelError(
                nameof(Contraseña),
                $"Demasiados intentos fallidos. Espera un minuto antes de volver a intentarlo.");

            return Page();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Usuario? usuario = _datos.LeerUsuario(EMail);

        if (usuario is null || !usuario.CompruebaPassword(Contraseña))
        {
            BloqueoHasta = RegistrarFallo(EMail);

            ModelState.AddModelError(
                nameof(Contraseña),
                BloqueoHasta is null
                    ? "Usuario o contraseña incorrectos."
                    : "Has alcanzado 3 intentos fallidos. Espera un minuto antes de volver a intentarlo.");

            return Page();
        }

        RegistrarExito(EMail);

        HttpContext.Session.Remove(ClavesSesion.EmailPendiente);
        HttpContext.Session.SetInt32(
            ClavesSesion.UsuarioId,
            usuario.IdUsuario);

        return RedirectToPage("/Principal/Index");
    }

    private DateTimeOffset? ObtenerBloqueoHasta(string email)
    {
        string clave = NormalizarEmail(email);

        lock (SincronizacionIntentos)
        {
            if (!IntentosPorEmail.TryGetValue(clave, out EstadoIntentos? estado) ||
                estado.BloqueoHasta is not DateTimeOffset hasta)
            {
                return null;
            }

            if (hasta <= DateTimeOffset.UtcNow)
            {
                IntentosPorEmail.Remove(clave);
                return null;
            }

            return hasta;
        }
    }

    private DateTimeOffset? RegistrarFallo(string email)
    {
        string clave = NormalizarEmail(email);
        DateTimeOffset ahora = DateTimeOffset.UtcNow;

        lock (SincronizacionIntentos)
        {
            if (IntentosPorEmail.TryGetValue(clave, out EstadoIntentos? estado) &&
                estado.BloqueoHasta is DateTimeOffset bloqueo)
            {
                if (bloqueo > ahora)
                {
                    return bloqueo;
                }

                IntentosPorEmail.Remove(clave);
            }

            if (!IntentosPorEmail.TryGetValue(clave, out estado))
            {
                estado = new EstadoIntentos();
                IntentosPorEmail[clave] = estado;
            }

            estado.Fallos++;

            if (estado.Fallos >= MaximoFallos)
            {
                estado.BloqueoHasta = ahora.Add(DuracionBloqueo);
                return estado.BloqueoHasta;
            }

            return null;
        }
    }

    private void RegistrarExito(string email)
    {
        lock (SincronizacionIntentos)
        {
            IntentosPorEmail.Remove(NormalizarEmail(email));
        }
    }

    private static string NormalizarEmail(string email) =>
        email.Trim().ToUpperInvariant();

    private string ObtenerEmailPendiente()
    {
        return HttpContext.Session.GetString(
            ClavesSesion.EmailPendiente) ?? "";
    }

    private sealed class EstadoIntentos
    {
        public int Fallos { get; set; }
        public DateTimeOffset? BloqueoHasta { get; set; }
    }
}