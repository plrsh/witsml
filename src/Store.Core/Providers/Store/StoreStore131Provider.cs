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

using System;
using System.Collections;
using System.ComponentModel.Composition;
using Energistics.Common;
using Energistics.DataAccess;
using Energistics.Datatypes;
using Energistics.Datatypes.Object;
using Energistics.Protocol.Store;
using PDS.WITSMLstudio.Framework;
using PDS.WITSMLstudio.Store.Data;

namespace PDS.WITSMLstudio.Store.Providers.Store
{
    /// <summary>
    /// Defines methods that can be used to perform CRUD operations via ETP for WITSML 1.3.1.1 objects.
    /// </summary>
    /// <seealso cref="PDS.WITSMLstudio.Store.Providers.Store.IStoreStoreProvider" />
    [Export131(typeof(IStoreStoreProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class StoreStore131Provider : IStoreStoreProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreStore131Provider" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        [ImportingConstructor]
        public StoreStore131Provider(IContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Gets the composition container.
        /// </summary>
        /// <value>The container.</value>
        public IContainer Container { get; private set; }

        /// <summary>
        /// Gets the data schema version supported by the provider.
        /// </summary>
        /// <value>The data schema version.</value>
        public string DataSchemaVersion
        {
            get { return OptionsIn.DataVersion.Version131.Value; }
        }

        /// <summary>
        /// Gets the object details for the specified URI.
        /// </summary>
        /// <param name="args">The <see cref="ProtocolEventArgs{GetObject, DataObject}" /> instance containing the event data.</param>
        public void GetObject(ProtocolEventArgs<GetObject, DataObject> args)
        {
            var uri = new EtpUri(args.Message.Uri);
            var dataAdapter = Container.Resolve<IEtpDataProvider>(new ObjectName(uri.ObjectType, uri.Version));
            var entity = dataAdapter.Get(uri) as IDataObject;
            var list = GetList(entity, uri);

            StoreStoreProvider.SetDataObject(args.Context, list, uri, GetName(entity), lastChanged: GetLastChanged(entity));
        }

        private IEnergisticsCollection GetList(IDataObject entity, EtpUri uri)
        {
            if (entity == null)
                return null;

            var groupType = ObjectTypes.GetObjectGroupType(uri.ObjectType, WMLSVersion.WITSML131);
            var property = ObjectTypes.GetObjectTypeListPropertyInfo(uri.ObjectType, uri.Version);

            var group = Activator.CreateInstance(groupType) as IEnergisticsCollection;
            var list = Activator.CreateInstance(property.PropertyType) as IList;
            if (list == null) return group;

            list.Add(entity);
            property.SetValue(group, list);

            return group;
        }

        private string GetName(IDataObject entity)
        {
            return entity == null ? string.Empty : entity.Name;
        }

        private long GetLastChanged(IDataObject entity)
        {
            var commonDataObject = entity as ICommonDataObject;
            var unixTime = commonDataObject?.CommonData.DateTimeLastChange.ToUnixTimeMicroseconds();

            return unixTime.GetValueOrDefault();
        }
    }
}
