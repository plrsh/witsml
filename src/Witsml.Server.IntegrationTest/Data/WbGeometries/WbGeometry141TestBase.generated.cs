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

// ----------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost
//     if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Energistics.DataAccess;
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.DataAccess.WITSML141.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WbGeometry = Energistics.DataAccess.WITSML141.StandAloneWellboreGeometry;
using WbGeometryList = Energistics.DataAccess.WITSML141.WellboreGeometryList;

namespace PDS.Witsml.Server.Data.WbGeometries
{
    public abstract partial class WbGeometry141TestBase : IntegrationTestBase
    {
        public const string QueryMissingNamespace = "<wbGeometrys version=\"1.4.1.1\"><wbGeometry /></wbGeometrys>";
        public const string QueryInvalidNamespace = "<wbGeometrys xmlns=\"www.witsml.org/schemas/123\" version=\"1.4.1.1\"></wbGeometrys>";
        public const string QueryMissingVersion = "<wbGeometrys xmlns=\"http://www.witsml.org/schemas/1series\"></wbGeometrys>";
        public const string QueryEmptyRoot = "<wbGeometrys xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"></wbGeometrys>";
        public const string QueryEmptyObject = "<wbGeometrys xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"><wbGeometry /></wbGeometrys>";
        public const string BasicXMLTemplate = "<wbGeometrys xmlns=\"http://www.witsml.org/schemas/1series\" version=\"1.4.1.1\"><wbGeometry uidWell=\"{0}\" uidWellbore=\"{1}\" uid=\"{2}\">{3}</wbGeometry></wbGeometrys>";

        public Well Well { get; set; }
        public Wellbore Wellbore { get; set; }
        public WbGeometry WbGeometry { get; set; }
        public DevKit141Aspect DevKit { get; set; }
        public List<WbGeometry> QueryEmptyList { get; set; }

        [TestInitialize]
        public void TestSetUp()
        {
            Logger.Debug($"Executing {TestContext.TestName}");
            DevKit = new DevKit141Aspect(TestContext);

            DevKit.Store.CapServerProviders = DevKit.Store.CapServerProviders
                .Where(x => x.DataSchemaVersion == OptionsIn.DataVersion.Version141.Value)
                .ToArray();

            Well = new Well
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Well"),
                TimeZone = DevKit.TimeZone
            };
            Wellbore = new Wellbore
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("Wellbore"),
                UidWell = Well.Uid,
                NameWell = Well.Name,
                MD = new MeasuredDepthCoord(0, MeasuredDepthUom.ft)
            };
            WbGeometry = new WbGeometry
            {
                Uid = DevKit.Uid(),
                Name = DevKit.Name("WbGeometry"),
                UidWell = Well.Uid,
                NameWell = Well.Name,
                UidWellbore = Wellbore.Uid,
                NameWellbore = Wellbore.Name
            };

            QueryEmptyList = DevKit.List(new WbGeometry());

            BeforeEachTest();
            OnTestSetUp();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            AfterEachTest();
            OnTestCleanUp();
            DevKit.Container.Dispose();
            DevKit = null;
        }

        partial void BeforeEachTest();

        partial void AfterEachTest();

        protected virtual void OnTestSetUp() { }

        protected virtual void OnTestCleanUp() { }

        protected virtual void AddParents()
        {
            DevKit.AddAndAssert<WellList, Well>(Well);
            DevKit.AddAndAssert<WellboreList, Wellbore>(Wellbore);
        }
    }
}