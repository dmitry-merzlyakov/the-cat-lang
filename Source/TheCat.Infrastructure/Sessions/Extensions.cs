using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TheCat.Infrastructure.VirtualFileSystem;
using System.Collections.Generic;
using System.Text;

namespace TheCat.Infrastructure.Sessions
{
    public static class Extensions
    {
        public static SessionDefinition GetSessionDefinitionForFile(this ISessionDefinitionRepository repository, string fileName)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");
            if (String.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException("fileName");

            SessionDefinition sessionDefinition = repository.GetDefaultSessionDefinition().Clone();
            sessionDefinition.InitModules.Add(fileName);

            return sessionDefinition;
        }

        public static SessionDefinition Clone(this SessionDefinition sessionDefinition)
        {
            SessionDefinition cloned = new SessionDefinition()
            {
                SessionDefinitionID = sessionDefinition.SessionDefinitionID,
                Name = sessionDefinition.Name,
                Description = sessionDefinition.Description,
                OutputStack = sessionDefinition.OutputStack,
                OutputTimeElapsed = sessionDefinition.OutputTimeElapsed,
                ShowWelcome = sessionDefinition.ShowWelcome
            };
            cloned.InitModules.AddRange(sessionDefinition.InitModules);
            cloned.InitCommands.AddRange(sessionDefinition.InitCommands);
            return cloned;
        }

        public static bool IsExitCommand(this string inputLine)
        {
            return !String.IsNullOrEmpty(inputLine) && (inputLine.ToLower() == "#exit" || inputLine.ToLower() == "#x");
        }

        public static string CreateStringFromList(this List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            list.ForEach(s => 
                {
                    if (!String.IsNullOrWhiteSpace(s))
                        sb.AppendLine(s.Trim());
                });
            return sb.ToString();
        }

        public static List<string> CreateListFromString(this string s)
        {
            return (s ?? string.Empty).Split(Separator, StringSplitOptions.RemoveEmptyEntries).Select(str => str.Trim()).Where(str => !String.IsNullOrWhiteSpace(str)).ToList();
        }

        private static string[] Separator = new string[] { "\r" };
    }
}
