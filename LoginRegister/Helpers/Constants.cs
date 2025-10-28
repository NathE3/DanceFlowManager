using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InfoManager.Helpers
{
    public static class Constants
    {
        public const string JSON_FILTER = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";

        public const string BASE_URL = "https://localhost:7777/api/";
        public const string PROYECTO_URL = "Proyecto";
        public const string PROYECTO_STATE_URL = "Proyecto/State";
        public const string LOGIN_PATH = "users/login";
        public const string REGISTER_PATH = "users/register";

    }
}
