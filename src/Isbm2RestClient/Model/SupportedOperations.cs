/*
 * ISBM 2.0
 *
 * An OpenAPI specification for the ISBM 2.0 RESTful interface.
 *
 * The version of the OpenAPI document: 2.0.1
 * Contact: info@mimosa.org
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = Isbm2RestClient.Client.OpenAPIDateConverter;

namespace Isbm2RestClient.Model
{
    /// <summary>
    /// Gets information about the supported operations and features of the ISBM service provider.
    /// </summary>
    [DataContract(Name = "SupportedOperations")]
    public partial class SupportedOperations : IEquatable<SupportedOperations>, IValidatableObject
    {

        /// <summary>
        /// Gets or Sets SecurityLevelConformance
        /// </summary>
        [DataMember(Name = "securityLevelConformance", IsRequired = true)]
        public SecurityLevel SecurityLevelConformance { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedOperations" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected SupportedOperations()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedOperations" /> class.
        /// </summary>
        /// <param name="isXMLFilteringEnabled">isXMLFilteringEnabled (required).</param>
        /// <param name="isJSONFilteringEnabled">isJSONFilteringEnabled (required).</param>
        /// <param name="supportedContentFilteringLanguages">supportedContentFilteringLanguages (required).</param>
        /// <param name="supportedAuthentications">supportedAuthentications (required).</param>
        /// <param name="securityLevelConformance">securityLevelConformance (required).</param>
        /// <param name="isDeadLetteringEnabled">isDeadLetteringEnabled (required).</param>
        /// <param name="isChannelCreationEnabled">isChannelCreationEnabled (required).</param>
        /// <param name="isOpenChannelSecuringEnabled">isOpenChannelSecuringEnabled (required).</param>
        /// <param name="isWhitelistRequired">isWhitelistRequired (required).</param>
        /// <param name="defaultExpiryDuration">Duration as defined by XML Schema xs:duration, http://w3c.org/TR/xmlschema-2/#duration, or  null (required).</param>
        /// <param name="additionalInformationURL">additionalInformationURL (required).</param>
        public SupportedOperations(bool isXMLFilteringEnabled = default(bool), bool isJSONFilteringEnabled = default(bool), SupportedOperationsSupportedContentFilteringLanguages supportedContentFilteringLanguages = default(SupportedOperationsSupportedContentFilteringLanguages), SupportedOperationsSupportedAuthentications supportedAuthentications = default(SupportedOperationsSupportedAuthentications), SecurityLevel securityLevelConformance = default(SecurityLevel), bool isDeadLetteringEnabled = default(bool), bool isChannelCreationEnabled = default(bool), bool isOpenChannelSecuringEnabled = default(bool), bool isWhitelistRequired = default(bool), string defaultExpiryDuration = default(string), string additionalInformationURL = default(string))
        {
            this.IsXMLFilteringEnabled = isXMLFilteringEnabled;
            this.IsJSONFilteringEnabled = isJSONFilteringEnabled;
            // to ensure "supportedContentFilteringLanguages" is required (not null)
            if (supportedContentFilteringLanguages == null)
            {
                throw new ArgumentNullException("supportedContentFilteringLanguages is a required property for SupportedOperations and cannot be null");
            }
            this.SupportedContentFilteringLanguages = supportedContentFilteringLanguages;
            // to ensure "supportedAuthentications" is required (not null)
            if (supportedAuthentications == null)
            {
                throw new ArgumentNullException("supportedAuthentications is a required property for SupportedOperations and cannot be null");
            }
            this.SupportedAuthentications = supportedAuthentications;
            this.SecurityLevelConformance = securityLevelConformance;
            this.IsDeadLetteringEnabled = isDeadLetteringEnabled;
            this.IsChannelCreationEnabled = isChannelCreationEnabled;
            this.IsOpenChannelSecuringEnabled = isOpenChannelSecuringEnabled;
            this.IsWhitelistRequired = isWhitelistRequired;
            // to ensure "defaultExpiryDuration" is required (not null)
            if (defaultExpiryDuration == null)
            {
                throw new ArgumentNullException("defaultExpiryDuration is a required property for SupportedOperations and cannot be null");
            }
            this.DefaultExpiryDuration = defaultExpiryDuration;
            // to ensure "additionalInformationURL" is required (not null)
            if (additionalInformationURL == null)
            {
                throw new ArgumentNullException("additionalInformationURL is a required property for SupportedOperations and cannot be null");
            }
            this.AdditionalInformationURL = additionalInformationURL;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets IsXMLFilteringEnabled
        /// </summary>
        [DataMember(Name = "isXMLFilteringEnabled", IsRequired = true)]
        public bool IsXMLFilteringEnabled { get; set; }

        /// <summary>
        /// Gets or Sets IsJSONFilteringEnabled
        /// </summary>
        [DataMember(Name = "isJSONFilteringEnabled", IsRequired = true)]
        public bool IsJSONFilteringEnabled { get; set; }

        /// <summary>
        /// Gets or Sets SupportedContentFilteringLanguages
        /// </summary>
        [DataMember(Name = "supportedContentFilteringLanguages", IsRequired = true)]
        public SupportedOperationsSupportedContentFilteringLanguages SupportedContentFilteringLanguages { get; set; }

        /// <summary>
        /// Gets or Sets SupportedAuthentications
        /// </summary>
        [DataMember(Name = "supportedAuthentications", IsRequired = true)]
        public SupportedOperationsSupportedAuthentications SupportedAuthentications { get; set; }

        /// <summary>
        /// Gets or Sets IsDeadLetteringEnabled
        /// </summary>
        [DataMember(Name = "isDeadLetteringEnabled", IsRequired = true)]
        public bool IsDeadLetteringEnabled { get; set; }

        /// <summary>
        /// Gets or Sets IsChannelCreationEnabled
        /// </summary>
        [DataMember(Name = "isChannelCreationEnabled", IsRequired = true)]
        public bool IsChannelCreationEnabled { get; set; }

        /// <summary>
        /// Gets or Sets IsOpenChannelSecuringEnabled
        /// </summary>
        [DataMember(Name = "isOpenChannelSecuringEnabled", IsRequired = true)]
        public bool IsOpenChannelSecuringEnabled { get; set; }

        /// <summary>
        /// Gets or Sets IsWhitelistRequired
        /// </summary>
        [DataMember(Name = "isWhitelistRequired", IsRequired = true)]
        public bool IsWhitelistRequired { get; set; }

        /// <summary>
        /// Duration as defined by XML Schema xs:duration, http://w3c.org/TR/xmlschema-2/#duration, or  null
        /// </summary>
        /// <value>Duration as defined by XML Schema xs:duration, http://w3c.org/TR/xmlschema-2/#duration, or  null</value>
        [DataMember(Name = "defaultExpiryDuration", IsRequired = true)]
        public string DefaultExpiryDuration { get; set; }

        /// <summary>
        /// Gets or Sets AdditionalInformationURL
        /// </summary>
        [DataMember(Name = "additionalInformationURL", IsRequired = true)]
        public string AdditionalInformationURL { get; set; }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class SupportedOperations {\n");
            sb.Append("  IsXMLFilteringEnabled: ").Append(IsXMLFilteringEnabled).Append("\n");
            sb.Append("  IsJSONFilteringEnabled: ").Append(IsJSONFilteringEnabled).Append("\n");
            sb.Append("  SupportedContentFilteringLanguages: ").Append(SupportedContentFilteringLanguages).Append("\n");
            sb.Append("  SupportedAuthentications: ").Append(SupportedAuthentications).Append("\n");
            sb.Append("  SecurityLevelConformance: ").Append(SecurityLevelConformance).Append("\n");
            sb.Append("  IsDeadLetteringEnabled: ").Append(IsDeadLetteringEnabled).Append("\n");
            sb.Append("  IsChannelCreationEnabled: ").Append(IsChannelCreationEnabled).Append("\n");
            sb.Append("  IsOpenChannelSecuringEnabled: ").Append(IsOpenChannelSecuringEnabled).Append("\n");
            sb.Append("  IsWhitelistRequired: ").Append(IsWhitelistRequired).Append("\n");
            sb.Append("  DefaultExpiryDuration: ").Append(DefaultExpiryDuration).Append("\n");
            sb.Append("  AdditionalInformationURL: ").Append(AdditionalInformationURL).Append("\n");
            sb.Append("  AdditionalProperties: ").Append(AdditionalProperties).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as SupportedOperations);
        }

        /// <summary>
        /// Returns true if SupportedOperations instances are equal
        /// </summary>
        /// <param name="input">Instance of SupportedOperations to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SupportedOperations input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.IsXMLFilteringEnabled == input.IsXMLFilteringEnabled ||
                    this.IsXMLFilteringEnabled.Equals(input.IsXMLFilteringEnabled)
                ) && 
                (
                    this.IsJSONFilteringEnabled == input.IsJSONFilteringEnabled ||
                    this.IsJSONFilteringEnabled.Equals(input.IsJSONFilteringEnabled)
                ) && 
                (
                    this.SupportedContentFilteringLanguages == input.SupportedContentFilteringLanguages ||
                    (this.SupportedContentFilteringLanguages != null &&
                    this.SupportedContentFilteringLanguages.Equals(input.SupportedContentFilteringLanguages))
                ) && 
                (
                    this.SupportedAuthentications == input.SupportedAuthentications ||
                    (this.SupportedAuthentications != null &&
                    this.SupportedAuthentications.Equals(input.SupportedAuthentications))
                ) && 
                (
                    this.SecurityLevelConformance == input.SecurityLevelConformance ||
                    this.SecurityLevelConformance.Equals(input.SecurityLevelConformance)
                ) && 
                (
                    this.IsDeadLetteringEnabled == input.IsDeadLetteringEnabled ||
                    this.IsDeadLetteringEnabled.Equals(input.IsDeadLetteringEnabled)
                ) && 
                (
                    this.IsChannelCreationEnabled == input.IsChannelCreationEnabled ||
                    this.IsChannelCreationEnabled.Equals(input.IsChannelCreationEnabled)
                ) && 
                (
                    this.IsOpenChannelSecuringEnabled == input.IsOpenChannelSecuringEnabled ||
                    this.IsOpenChannelSecuringEnabled.Equals(input.IsOpenChannelSecuringEnabled)
                ) && 
                (
                    this.IsWhitelistRequired == input.IsWhitelistRequired ||
                    this.IsWhitelistRequired.Equals(input.IsWhitelistRequired)
                ) && 
                (
                    this.DefaultExpiryDuration == input.DefaultExpiryDuration ||
                    (this.DefaultExpiryDuration != null &&
                    this.DefaultExpiryDuration.Equals(input.DefaultExpiryDuration))
                ) && 
                (
                    this.AdditionalInformationURL == input.AdditionalInformationURL ||
                    (this.AdditionalInformationURL != null &&
                    this.AdditionalInformationURL.Equals(input.AdditionalInformationURL))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && !this.AdditionalProperties.Except(input.AdditionalProperties).Any());
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                hashCode = (hashCode * 59) + this.IsXMLFilteringEnabled.GetHashCode();
                hashCode = (hashCode * 59) + this.IsJSONFilteringEnabled.GetHashCode();
                if (this.SupportedContentFilteringLanguages != null)
                {
                    hashCode = (hashCode * 59) + this.SupportedContentFilteringLanguages.GetHashCode();
                }
                if (this.SupportedAuthentications != null)
                {
                    hashCode = (hashCode * 59) + this.SupportedAuthentications.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.SecurityLevelConformance.GetHashCode();
                hashCode = (hashCode * 59) + this.IsDeadLetteringEnabled.GetHashCode();
                hashCode = (hashCode * 59) + this.IsChannelCreationEnabled.GetHashCode();
                hashCode = (hashCode * 59) + this.IsOpenChannelSecuringEnabled.GetHashCode();
                hashCode = (hashCode * 59) + this.IsWhitelistRequired.GetHashCode();
                if (this.DefaultExpiryDuration != null)
                {
                    hashCode = (hashCode * 59) + this.DefaultExpiryDuration.GetHashCode();
                }
                if (this.AdditionalInformationURL != null)
                {
                    hashCode = (hashCode * 59) + this.AdditionalInformationURL.GetHashCode();
                }
                if (this.AdditionalProperties != null)
                {
                    hashCode = (hashCode * 59) + this.AdditionalProperties.GetHashCode();
                }
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            // DefaultExpiryDuration (string) pattern
            Regex regexDefaultExpiryDuration = new Regex(@"[-]?P([0-9]+Y)?([0-9]+M)?([0-9]+D)?(T([0-9]+H)?([0-9]+M)?([0-9]+([.][0-9]+)?S)?)?", RegexOptions.CultureInvariant);
            if (false == regexDefaultExpiryDuration.Match(this.DefaultExpiryDuration).Success)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for DefaultExpiryDuration, must match a pattern of " + regexDefaultExpiryDuration, new [] { "DefaultExpiryDuration" });
            }

            yield break;
        }
    }

}
