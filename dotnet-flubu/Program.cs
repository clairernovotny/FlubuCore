﻿using flubu.Infrastructure;
using Microsoft.DotNet.ProjectModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace flubu
{
    public class Program
    {
        private static IServiceCollection _services = new ServiceCollection();
        private static IServiceProvider _provider;

#pragma warning disable RECS0154 // Parameter is never used
        public static int Main(string[] args)
#pragma warning restore RECS0154 // Parameter is never used
        {
            _services.RegisterAll();

            _provider = _services.BuildServiceProvider();
            //Compiler.Compile();

            return 0;

            //DotnetCommandParams parameters = new DotnetCommandParams();

            //parameters.Parse(args);

            //if (parameters.Help)
            //{
            //    return 0;
            //}

            //if (string.IsNullOrEmpty(parameters.Command))
            //{
            //    Reporter.Error.WriteLine("Missing argument <COMMAND>");
            //    parameters.ShowHint();
            //    return 1;
            //}

            //if (parameters.ParentProcessId.HasValue)
            //{
            //    RegisterForParentProcessExit(parameters.ParentProcessId.Value);
            //}

            //IEnumerable<ProjectContext> projectContexts = CreateProjectContexts(parameters.ProjectPath);
            //ProjectContext projectContext = parameters.Framework != null
            //    ? projectContexts.First(p => p.TargetFramework == parameters.Framework)
            //    : projectContexts.First();

            //var runner = new CommandRunner(projectContext);

            //return runner.Run(parameters);
        }

        private static IEnumerable<ProjectContext> CreateProjectContexts(string projectPath)
        {
            projectPath = projectPath ?? Directory.GetCurrentDirectory();

            if (!projectPath.EndsWith(Project.FileName, StringComparison.Ordinal))
            {
                projectPath = Path.Combine(projectPath, Project.FileName);
            }

            if (!File.Exists(projectPath))
            {
                throw new InvalidOperationException($"{projectPath} does not exist.");
            }

            return ProjectContext.CreateContextForEachFramework(projectPath);
        }

        private static void RegisterForParentProcessExit(int id)
        {
            Process parentProcess = Process.GetProcesses().FirstOrDefault(p => p.Id == id);

            if (parentProcess != null)
            {
                parentProcess.EnableRaisingEvents = true;
                parentProcess.Exited += (sender, eventArgs) =>
                {
                    Process.GetCurrentProcess().Kill();
                };
            }
        }
    }
}