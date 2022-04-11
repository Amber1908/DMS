using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Text;

namespace X1APServer.Repository.Utility
{
    public class EntityExceptionHandler
    {
        public static bool IsDbEntityValidationException(Exception ex)
        {
            return ex is DbEntityValidationException;
        }

        public static string Convert(Exception ex)
        {
            List<string> messages = new List<string>();

            if (IsDbEntityValidationException(ex))
            {
                var exception = ex as DbEntityValidationException;

                foreach (var evr in exception.EntityValidationErrors)
                {
                    messages.Add(string.Format(
                        "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        evr.Entry.Entity.GetType().Name,
                        evr.Entry.State
                    ));

                    foreach (var ve in evr.ValidationErrors)
                    {
                        messages.Add(string.Format(
                            "- {0} : {1}",
                            ve.PropertyName,
                            ve.ErrorMessage
                        ));
                    }
                }
            }

            return string.Join(Environment.NewLine, messages);
        }

        public static string HandleDbUpdateException(Exception ex)
        {
            var builder = new StringBuilder();
            if (ex is DbUpdateException)
            {
                var dbu = ex as DbUpdateException;
                builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

                try
                {
                    foreach (var result in dbu.Entries)
                    {
                        builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                    }
                }
                catch (Exception e)
                {
                    builder.Append("Error parsing DbUpdateException: " + e.ToString());
                }

                string message = builder.ToString();
                return message;
            }
            else
            {
                return builder.ToString();
            }
        }
    }
}
