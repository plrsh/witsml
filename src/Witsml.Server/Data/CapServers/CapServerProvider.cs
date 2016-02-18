﻿using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Energistics.DataAccess;

namespace PDS.Witsml.Server.Data.CapServers
{
    /// <summary>
    /// Provides common WTISML server capabilities for any data schema version.
    /// </summary>
    /// <typeparam name="T">The capServers type.</typeparam>
    /// <seealso cref="PDS.Witsml.Server.Data.ICapServerProvider" />
    public abstract class CapServerProvider<T> : ICapServerProvider
    {
        private T _capServer;
        private XDocument _capServerDoc;
        private string _capServerXml;

        /// <summary>
        /// Gets the data schema version.
        /// </summary>
        /// <value>The data schema version.</value>
        public abstract string DataSchemaVersion { get; }

        /// <summary>
        /// Returns the server capabilities object as XML.
        /// </summary>
        /// <returns>A capServers object as an XML string.</returns>
        public string ToXml()
        {
            if (!string.IsNullOrWhiteSpace(_capServerXml))
            {
                return _capServerXml;
            }

            var capServer = GetCapServer();

            if (capServer != null)
            {
                _capServerXml = EnergisticsConverter.ObjectToXml(capServer);
            }

            return _capServerXml;
        }

        /// <summary>
        /// Determines whether the specified function is supported for the object type.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// true if the WITSML Store supports the function for the specified object type, otherwise, false
        /// </returns>
        public bool IsSupported(Functions function, string objectType)
        {
            var capServerDoc = GetCapServerDocument();
            var ns = XNamespace.Get(capServerDoc.Root.CreateNavigator().GetNamespace(string.Empty));

            return capServerDoc.Descendants(ns + "dataObject")
                .Where(x => x.Value == objectType && x.Parent.Attribute("name").Value == "WMLS_" + function)
                .Any();
        }

        /// <summary>
        /// Creates the capServers instance for a specific data schema version.
        /// </summary>
        /// <returns>The capServers instance.</returns>
        protected abstract T CreateCapServer();

        /// <summary>
        /// Gets the cached capServers instance or creates a new one.
        /// </summary>
        /// <returns>The capServers instance.</returns>
        private T GetCapServer()
        {
            if (_capServer != null)
            {
                return _capServer;
            }

            _capServer = CreateCapServer();

            return _capServer;
        }

        /// <summary>
        /// Gets the cached capServers object as an <see cref="XDocument"/>.
        /// </summary>
        /// <returns>The <see cref="XDocument"/> instance.</returns>
        private XDocument GetCapServerDocument()
        {
            if (_capServerDoc != null)
            {
                return _capServerDoc;
            }

            _capServerDoc = XDocument.Parse(ToXml());

            return _capServerDoc;
        }
    }
}