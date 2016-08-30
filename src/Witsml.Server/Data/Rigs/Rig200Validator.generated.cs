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

using System.ComponentModel.Composition;
using Energistics.DataAccess.WITSML200;
using PDS.Framework;


namespace PDS.Witsml.Server.Data.Rigs
{
    /// <summary>
    /// Provides validation for <see cref="Rig" /> data objects.
    /// </summary>
    /// <seealso cref="PDS.Witsml.Server.Data.DataObjectValidator{Rig}" />
    [Export(typeof(IDataObjectValidator<Rig>))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Rig200Validator : DataObjectValidator<Rig>
    {
        private readonly IWitsmlDataAdapter<Rig> _rigDataAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rig200Validator" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="rigDataAdapter">The rig data adapter.</param>
        [ImportingConstructor]
        public Rig200Validator(
            IContainer container,
            IWitsmlDataAdapter<Rig> rigDataAdapter)
            : base(container)
        {
            _rigDataAdapter = rigDataAdapter;
        }
    }
}