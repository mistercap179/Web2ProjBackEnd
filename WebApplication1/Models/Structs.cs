using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Structs
    {
        public struct JsonReturnUserExist
        {
            public string userName;
            public string email;
        }

        public class JsonResponseLogin
        {
            public string token;
            public string role;
            public Osoba osoba;
        }

        public class JsonPorudzbina
        {
            public int IdPotrosac;
        }

        public class JsonPorudzbinaDostavljac
        {
            public int IdDostavljac;
        }

        public class JsonDostava
        {
            public int IdPorudzbina;
            public Models_Backend.Dostavljac dostavljac;
        }

        public class JsonZavrseno
        {
            public int IdPorudzbina;
        }


        public class JsonBlokiran
        {
            public int status;
        }


        public class StatusResponse
        {
            public int status;

            public StatusResponse(int status)
            {
                this.status = status;
            }
        }

        public class JsonResponseLoginFail
        {
            public string message;
        }

        public class JsonResponsePasswordFail
        {
            public string poruka;
        }

    }
}
