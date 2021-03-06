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
using System.Reflection;
using System.Globalization;

namespace Cat
{
    public static class WindowGDI
    {
        public static void ClearWindow()
        {
            CatEnvironment.GraphicConsole.ClearWindow();
        }

        public static void CloseWindow()
        {
            CatEnvironment.GraphicConsole.CloseWindow();
        }

        public static void SaveToFile(string fileName)
        {
            CatEnvironment.GraphicConsole.SaveToFile(fileName);
        }

        public static void OpenWindow()
        {
            CatEnvironment.GraphicConsole.OpenWindow();
        }

        public static void Render(GraphicCommand cmd)
        {
            CatEnvironment.GraphicConsole.Render(cmd);
        }
    }

    public class GraphicCommand
    {
        public GraphicCommand(string sCmd, Object[] args)
        {
            msCommand = sCmd;
            maArgs = args;
            maArgTypes = new Type[maArgs.Length];
            for (int i = 0; i < maArgs.Length; ++i)
                maArgTypes[i] = maArgs[i].GetType();
        }
        public MethodInfo GetMethod(Type t)
        {
            MethodInfo ret = t.GetMethod(msCommand);
            if (ret == null)
            {
                ret = t.GetMethod(msCommand, GetArgTypes());
                if (ret == null)
                    throw new Exception("Could not find method " + msCommand + " on type " + t.ToString() + " with matching types");
            }
            return ret;
        }
        public Type[] GetArgTypes()
        {
            return maArgTypes;
        }
        public string GetMethodName()
        {
            return msCommand;
        }
        public Object[] GetArgs()
        {
            return maArgs;
        }
        string msCommand;
        Object[] maArgs;
        Type[] maArgTypes;

        public void Invoke(Object o, Type t)
        {
            MethodInfo mi = GetMethod(t);

            // DM+ This is a quick fix for the argument type issues. It is subject for optimization - TODO
            CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            ParameterInfo[] parameterInfos = mi.GetParameters();
            object[] arguments = new object[parameterInfos.Length];
            for (int index = 0; index < parameterInfos.Length && index < maArgs.Length; index++)
            {
                arguments[index] = Convert.ChangeType(maArgs[index], parameterInfos[index].ParameterType, culture);
            }
            mi.Invoke(o, arguments);
            //DM-
            //mi.Invoke(o, maArgs);
        }
    }

}
