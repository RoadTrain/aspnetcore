// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Xunit;

namespace Microsoft.AspNetCore.Builder
{
    public class RoutingEndpointConventionBuilderExtensionsTest
    {
        [Fact]
        public void RequireHost_AddsHostMetadata()
        {
            // Arrange
            var builder = CreateBuilder();

            // Act
            builder.RequireHost("www.example.com", "example.com");

            // Assert
            var endpoint = builder.Build();

            var metadata = endpoint.Metadata.GetMetadata<IHostMetadata>();
            Assert.NotNull(metadata);
            Assert.Equal(new[] { "www.example.com", "example.com" }, metadata.Hosts);
        }

        [Fact]
        public void RequireHost_ChainedCall_ReturnedBuilderIsDerivedType()
        {
            // Arrange
            var builder = CreateBuilder();

            // Act
            var chainedBuilder = builder.RequireHost("test");

            // Assert
            Assert.True(chainedBuilder.TestProperty);
        }

        [Fact]
        public void WithDisplayName_String_SetsDisplayName()
        {
            // Arrange
            var builder = CreateBuilder();

            // Act
            builder.WithDisplayName("test");

            // Assert
            var endpoint = builder.Build();
            Assert.Equal("test", endpoint.DisplayName);
        }

        [Fact]
        public void WithDisplayName_ChainedCall_ReturnedBuilderIsDerivedType()
        {
            // Arrange
            var builder = CreateBuilder();

            // Act
            var chainedBuilder = builder.WithDisplayName("test");

            // Assert
            Assert.True(chainedBuilder.TestProperty);
        }

        [Fact]
        public void WithDisplayName_Func_SetsDisplayName()
        {
            // Arrange
            var builder = CreateBuilder();

            // Act
            builder.WithDisplayName(b => "test");

            // Assert
            var endpoint = builder.Build();
            Assert.Equal("test", endpoint.DisplayName);
        }

        [Fact]
        public void WithMetadata_AddsMetadata()
        {
            // Arrange
            var builder = CreateBuilder();

            // Act
            builder.WithMetadata("test", new HostAttribute("www.example.com", "example.com"));

            // Assert
            var endpoint = builder.Build();

            var hosts = endpoint.Metadata.GetMetadata<IHostMetadata>();
            Assert.NotNull(hosts);
            Assert.Equal(new[] { "www.example.com", "example.com" }, hosts.Hosts);

            var @string = endpoint.Metadata.GetMetadata<string>();
            Assert.Equal("test", @string);
        }

        [Fact]
        public void WithMetadata_ChainedCall_ReturnedBuilderIsDerivedType()
        {
            // Arrange
            var builder = CreateBuilder();

            // Act
            var chainedBuilder = builder.WithMetadata("test");

            // Assert
            Assert.True(chainedBuilder.TestProperty);
        }

        [Fact]
        public void WithName_SetsEndpointName()
        {
            // Arrange
            var builder = CreateBuilder();

            // Act
            builder.WithName("SomeEndpointName");

            // Assert
            var endpoint = builder.Build();

            var endpointName = endpoint.Metadata.GetMetadata<IEndpointNameMetadata>();
            Assert.Equal("SomeEndpointName", endpointName.EndpointName);
        }

        [Fact]
        public void WithGroupName_SetsEndpointGroupName()
        {
            // Arrange
            var builder = CreateBuilder();

            // Act
            builder.WithGroupName("SomeEndpointGroupName");

            // Assert
            var endpoint = builder.Build();

            var endpointGroupName = endpoint.Metadata.GetMetadata<IEndpointGroupNameMetadata>();
            Assert.Equal("SomeEndpointGroupName", endpointGroupName.EndpointGroupName);
        }

        private TestEndpointConventionBuilder CreateBuilder()
        {
            var conventionBuilder = new DefaultEndpointConventionBuilder(new RouteEndpointBuilder(
                TestConstants.EmptyRequestDelegate,
                RoutePatternFactory.Parse("/test"),
                order: 0));

            return new TestEndpointConventionBuilder(conventionBuilder);
        }

        private class TestEndpointConventionBuilder : IEndpointConventionBuilder
        {
            private readonly DefaultEndpointConventionBuilder _endpointConventionBuilder;
            public bool TestProperty { get; } = true;

            public TestEndpointConventionBuilder(DefaultEndpointConventionBuilder endpointConventionBuilder)
            {
                _endpointConventionBuilder = endpointConventionBuilder;
            }

            public void Add(Action<EndpointBuilder> convention)
            {
                _endpointConventionBuilder.Add(convention);
            }

            public Endpoint Build()
            {
                return _endpointConventionBuilder.Build();
            }
        }
    }
}
