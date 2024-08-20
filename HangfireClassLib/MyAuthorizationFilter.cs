using Hangfire.Dashboard;
using System.Net;
using System.Text;

namespace HangfireClassLib
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private static bool _isAuthorized = false;
        private static readonly object _lock = new object();
        public bool Authorize(DashboardContext context)
        {
            lock (_lock)
            {
                if (_isAuthorized)
                {
                    return true; // Already authorized, skip further checks
                }
                string authHeader = context.GetHttpContext().Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the encoded username and password
                    var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                    // Decode from Base64 to string
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    // Split username and password
                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];

                    bool isUserAuthorized=false;
                    // Check if login is correct
                    if (IsAuthorized(username, password))
                    {
                        _isAuthorized = true; // Set the flag to true after successful authorization
                        
                        return true;
                    }
                    return isUserAuthorized;
                }

                // Return authentication type (causes browser to show login dialog)
                context.GetHttpContext().Response.Headers["WWW-Authenticate"] = "Basic";

                // Return unauthorized
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return false;
            }
        }

        private bool IsAuthorized(string username, string password)
        {
            // Check that username and password are correct
            return username.Equals("admin", StringComparison.InvariantCultureIgnoreCase)
                    && password.Equals("admin@123");
        }
    }
}
