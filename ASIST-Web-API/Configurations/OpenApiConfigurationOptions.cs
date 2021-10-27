using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace ASIST_Web_API.Configurations {
	public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions {
		public override OpenApiInfo Info { get; set; } = new OpenApiInfo() {
			Version = "3.0.0",
			Title = "ASIST Project Azure functions app",
			Description = "This is the azure functions app for the ASIST project for the cloud computing minor",
			TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
			Contact = new OpenApiContact() {
				Name = "Project Group 9",
				Email = "627650@student.inholland.nl + 622796@student.inholland.nl",
				//Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
			},
			License = new OpenApiLicense() {
				Name = "MIT",
				Url = new Uri("http://opensource.org/licenses/MIT"),
			}
		};

		public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
	}
}
