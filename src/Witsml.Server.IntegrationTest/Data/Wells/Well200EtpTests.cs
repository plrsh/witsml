//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDS.Witsml.Server.Data.Wells
{
    /// <summary>
    /// Well200EtpTests
    /// </summary>
    public partial class Well200EtpTests
    {
        [TestMethod]
        public async Task Well200_GetResources_Can_Get_Root_Level_Resources()
        {
            AddParents();
            DevKit.AddAndAssert(Well);

            await RequestSessionAndAssert();
            await GetResourcesAndAssert(EtpUris.Witsml200);
        }
    }
}