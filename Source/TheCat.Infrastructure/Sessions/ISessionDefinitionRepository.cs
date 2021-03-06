﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TheCat.Infrastructure.Sessions
{
    public interface ISessionDefinitionRepository : IBaseRepository<SessionDefinition,string>
    {
        SessionDefinition GetDefaultSessionDefinition();
        void SaveDefaultSessionDefinition(SessionDefinition sessionDefinition);
    }
}
