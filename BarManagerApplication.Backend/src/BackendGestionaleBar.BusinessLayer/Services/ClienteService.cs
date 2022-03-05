using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.Shared.Helpers;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IDatabase db;

        public ClienteService(IDatabase db)
        {
            this.db = db;
        }

        public async Task<Response> RegisterClienteAsync(RegisterClienteRequest request)
        {
            List<string> errors = ParametersHelper.CheckRegisterClienteParameters(request);
            if (errors.Count > 0)
            {
                return new Response
                {
                    Succeeded = false,
                    Errors = errors
                };
            }

            var cliente = new Cliente
            {
                IdCliente = Guid.NewGuid(),
                Nome = request.Nome,
                Cognome = request.Cognome,
                DataNascita = request.DataNascita.Value,
                CodiceFiscale = request.CodiceFiscale,
                Telefono = request.Telefono
            };

            var result = await db.RegisterClienteAsync(cliente);

            if (result > 0)
            {
                return new Response
                {
                    Succeeded = true,
                    Errors = null
                };
            }
            else
            {
                return new Response
                {
                    Succeeded = false,
                    Errors = new List<string>
                    {
                        "Errors during registration"
                    }
                };
            }
        }
    }
}