﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Store, 2018.1
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

// ----------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost
//     if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Energistics.DataAccess.WITSML200;
using Energistics.DataAccess.WITSML200.ComponentSchemas;
using Energistics.Datatypes;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Store.Data.WellboreCompletions
{
    /// <summary>
    /// Data provider that implements support for WITSML API functions for <see cref="WellboreCompletion"/>.
    /// </summary>

    /// <seealso cref="PDS.WITSMLstudio.Store.Data.EtpDataProvider{WellboreCompletion}" />
    [Export(typeof(IEtpDataProvider))]
    [Export(typeof(IEtpDataProvider<WellboreCompletion>))]
    [Export200(ObjectTypes.WellboreCompletion, typeof(IEtpDataProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class WellboreCompletion200DataProvider : EtpDataProvider<WellboreCompletion>

    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WellboreCompletion200DataProvider"/> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="dataAdapter">The data adapter.</param>
        [ImportingConstructor]
        public WellboreCompletion200DataProvider(IContainer container, IWitsmlDataAdapter<WellboreCompletion> dataAdapter) : base(container, dataAdapter)
        {
        }

        /// <summary>
        /// Sets the default values for the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        protected override void SetDefaultValues(WellboreCompletion dataObject)
        {
            base.SetDefaultValues(dataObject);
            SetAdditionalDefaultValues(dataObject);
        }

        /// <summary>
        /// Sets the default values for the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="uri">The data object URI.</param>
        protected override void SetDefaultValues(WellboreCompletion dataObject, EtpUri uri)
        {
            base.SetDefaultValues(dataObject, uri);

            SetAdditionalDefaultValues(dataObject, uri);
        }

        /// <summary>
        /// Sets the default values for the specified data object during update.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="parser">The input template.</param>
        protected override void UpdateDefaultValues(WellboreCompletion dataObject, WitsmlQueryParser parser)
        {
            UpdateAdditionalDefaultValues(dataObject, parser);
        }

        /// <summary>
        /// Sets additional default values for the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        partial void SetAdditionalDefaultValues(WellboreCompletion dataObject);

        /// <summary>
        /// Sets additional default values for the specified data object and URI.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="uri">The data object URI.</param>
        partial void SetAdditionalDefaultValues(WellboreCompletion dataObject, EtpUri uri);

        /// <summary>
        /// Sets additional default values for the specified data object during update.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="parser">The input template.</param>
        partial void UpdateAdditionalDefaultValues(WellboreCompletion dataObject, WitsmlQueryParser parser);
    }
}
