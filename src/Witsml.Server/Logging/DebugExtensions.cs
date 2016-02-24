﻿using System;
using System.ServiceModel.Web;
using System.Xml.Linq;
using log4net;

namespace PDS.Witsml.Server.Logging
{
    /// <summary>
    /// Provides a set of helper methods useful for logging WITSML Store requests and responses.
    /// </summary>
    public static class DebugExtensions
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(WitsmlStore));

        /// <summary>
        /// Formats request information into a message suitable for logging.
        /// </summary>
        /// <param name="context">The web operation context.</param>
        /// <param name="isEnabled">if set to <c>true</c> the message is created.</param>
        /// <returns>The string representation of the request.</returns>
        public static string ToLogMessage(this WebOperationContext context, bool isEnabled = false)
        {
            if (context == null || context.IncomingRequest == null)
                return string.Empty;

            if (!_log.IsDebugEnabled && !isEnabled)
                return string.Empty;

            return string.Format(
                "UserAgent: {0}",
                context.IncomingRequest.UserAgent);
        }

        /// <summary>
        /// Converts the request to a message suitable for logging.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="isEnabled">if set to <c>true</c> the message is created.</param>
        /// <returns>The string representation of the request.</returns>
        public static string ToLogMessage(this WMLS_GetVersionRequest request, bool isEnabled = false)
        {
            if (!_log.IsDebugEnabled && !isEnabled)
                return string.Empty;

            return string.Format(
                "{0}",
                request.GetType().Name);
        }

        /// <summary>
        /// Converts the response to a message suitable for logging.
        /// </summary>
        /// <param name="response">The response object.</param>
        /// <param name="isEnabled">if set to <c>true</c> the message is created.</param>
        /// <returns>The string representation of the response.</returns>
        public static string ToLogMessage(this WMLS_GetVersionResponse response, bool isEnabled = false)
        {
            if (!_log.IsDebugEnabled && !isEnabled)
                return string.Empty;

            return string.Format(
                "{0}: Result: {1}",
                response.GetType().Name,
                response.Result);
        }

        /// <summary>
        /// Converts the request to a message suitable for logging.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="isEnabled">if set to <c>true</c> the message is created.</param>
        /// <returns>The string representation of the request.</returns>
        public static string ToLogMessage(this WMLS_GetCapRequest request, bool isEnabled = false)
        {
            if (!_log.IsDebugEnabled && !isEnabled)
                return string.Empty;

            return string.Format(
                "{0}: Options: {1}",
                request.GetType().Name,
                request.OptionsIn);
        }

        /// <summary>
        /// Converts the response to a message suitable for logging.
        /// </summary>
        /// <param name="response">The response object.</param>
        /// <param name="isEnabled">if set to <c>true</c> the message is created.</param>
        /// <returns>The string representation of the response.</returns>
        public static string ToLogMessage(this WMLS_GetCapResponse response, bool isEnabled = false)
        {
            if (!_log.IsDebugEnabled && !isEnabled)
                return string.Empty;

            return string.Format(
                "{0}: Result: {1}; Message: {2}; XML:{4}{3}{4}",
                response.GetType().Name,
                response.Result,
                response.SuppMsgOut,
                response.CapabilitiesOut,
                Environment.NewLine);
        }

        /// <summary>
        /// Converts the request to a message suitable for logging.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="isEnabled">if set to <c>true</c> the message is created.</param>
        /// <returns>The string representation of the request.</returns>
        public static string ToLogMessage(this WMLS_GetFromStoreRequest request, bool isEnabled = false)
        {
            if (!_log.IsDebugEnabled && !isEnabled)
                return string.Empty;

            return string.Format(
                "{0}: Type: {1}; Options: {2}; CapClient:{5}{3}{5}XML:{5}{4}{5}",
                request.GetType().Name,
                request.WMLtypeIn,
                request.OptionsIn,
                request.CapabilitiesIn,
                Format(request.QueryIn),
                Environment.NewLine);
        }

        /// <summary>
        /// Converts the response to a message suitable for logging.
        /// </summary>
        /// <param name="response">The response object.</param>
        /// <param name="isEnabled">if set to <c>true</c> the message is created.</param>
        /// <returns>The string representation of the response.</returns>
        public static string ToLogMessage(this WMLS_GetFromStoreResponse response, bool isEnabled = false)
        {
            if (!_log.IsDebugEnabled && !isEnabled)
                return string.Empty;

            return string.Format(
                "{0}: Result: {1}; Message: {2}; XML:{4}{3}{4}",
                response.GetType().Name,
                response.Result,
                response.SuppMsgOut,
                response.XMLout,
                Environment.NewLine);
        }

        /// <summary>
        /// Converts the request to a message suitable for logging.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="isEnabled">if set to <c>true</c> the message is created.</param>
        /// <returns>The string representation of the request.</returns>
        public static string ToLogMessage(this WMLS_AddToStoreRequest request, bool isEnabled = false)
        {
            if (!_log.IsDebugEnabled && !isEnabled)
                return string.Empty;

            return string.Format(
                "{0}: Type: {1}; Options: {2}; CapClient:{5}{3}{5}XML:{5}{4}{5}",
                request.GetType().Name,
                request.WMLtypeIn,
                request.OptionsIn,
                request.CapabilitiesIn,
                Format(request.XMLin),
                Environment.NewLine);
        }

        /// <summary>
        /// Converts the response to a message suitable for logging.
        /// </summary>
        /// <param name="response">The response object.</param>
        /// <param name="isEnabled">if set to <c>true</c> the message is created.</param>
        /// <returns>The string representation of the response.</returns>
        public static string ToLogMessage(this WMLS_AddToStoreResponse response, bool isEnabled = false)
        {
            if (!_log.IsDebugEnabled && !isEnabled)
                return string.Empty;

            return string.Format(
                "{0}: Result: {1}; Message: {2}",
                response.GetType().Name,
                response.Result,
                response.SuppMsgOut);
        }

        private static string Format(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return string.Empty;

            try
            {
                return XDocument.Parse(xml).ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}