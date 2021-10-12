using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace ASIST.Configurations {
	public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions {
		public override OpenApiInfo Info { get; set; } = new OpenApiInfo() {
			Version = "3.0.0",
			Title = "ASIST Project Web Api",
			Description = "This is the Web API for the ASIST project which lists the endpoints",
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
