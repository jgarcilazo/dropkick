// Copyright 2007-2010 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace dropkick.Engine.DeploymentFinders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Configuration.Dsl;
    using log4net;

    public class AssemblyWasSpecifiedAssumingOnlyOneDeploymentClass :
        DeploymentFinder
    {
        static readonly ILog _log = LogManager.GetLogger(typeof (AssemblyWasSpecifiedAssumingOnlyOneDeploymentClass));

        #region DeploymentFinder Members

        public Deployment Find(string assemblyName)
        {
            //check that it is an assembly

            string path = FindFile(assemblyName);

            Assembly asm = Assembly.LoadFile(path);
            IEnumerable<Type> tt = asm.GetTypes().Where(t => typeof (Deployment).IsAssignableFrom(t));

            return new TypeWasSpecifiedAssumingItHasADefaultConstructor().Find(tt.First());
        }

        #endregion

        string FindFile(string file)
        {
            string p = Path.Combine(Environment.CurrentDirectory, file);
            _log.DebugFormat("Looking for deployment dll '{0}' at '{1}'", file, p);

            if (!File.Exists(p))
                throw new FileNotFoundException("Couldn't Find File", p);

            return p;
        }
    }
}