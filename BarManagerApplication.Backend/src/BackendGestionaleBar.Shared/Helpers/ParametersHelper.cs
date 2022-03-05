using BackendGestionaleBar.Shared.Models.Requests;
using System.Collections.Generic;

namespace BackendGestionaleBar.Shared.Helpers
{
    public static class ParametersHelper
    {
        public static List<string> CheckRegisterClienteParameters(RegisterClienteRequest request)
        {
            List<string> errors = new();

            if (string.IsNullOrEmpty(request.Nome))
            {
                errors.Add("il nome è richiesto");
            }
            if (string.IsNullOrEmpty(request.Cognome))
            {
                errors.Add("il cognome è richiesto");
            }
            if (request.DataNascita == null)
            {
                errors.Add("la data di nascita è richiesta");
            }
            if (string.IsNullOrEmpty(request.Telefono))
            {
                errors.Add("il telefono è richiesto");
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                errors.Add("l'email è richiesta");
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                errors.Add("la password è richiesta");
            }

            return errors;
        }
    }
}