﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AudioToText;
using Microsoft.SemanticKernel.Connectors.AssemblyAI;
using Xunit;

namespace SemanticKernel.Connectors.UnitTests.AssemblyAI;

/// <summary>
/// Unit tests for <see cref="AssemblyAIServiceCollectionExtensions"/> class.
/// </summary>
public sealed class AssemblyAIAudioToTextServiceExtensionsTests
{
    private const string ApiKey = "Test123";
    private const string Endpoint = "http://localhost:1234/";
    private const string ServiceId = "AssemblyAI";

    [Fact]
    public void AddServiceToKernelBuilder()
    {
        // Arrange & Act
        using var httpClient = new HttpClient();
        var kernel = Kernel.CreateBuilder()
            .AddAssemblyAIAudioToText(
                apiKey: ApiKey,
                endpoint: new Uri(Endpoint),
                serviceId: ServiceId,
                httpClient: httpClient
            )
            .Build();

        // Assert
        var service = kernel.GetRequiredService<IAudioToTextService>();
        Assert.NotNull(service);
        Assert.IsType<AssemblyAIAudioToTextService>(service);

        service = kernel.GetRequiredService<IAudioToTextService>(ServiceId);
        Assert.NotNull(service);
        Assert.IsType<AssemblyAIAudioToTextService>(service);
    }

    [Fact]
    public void AddServiceToServiceCollection()
    {
        // Arrange & Act
        var services = new ServiceCollection();
        services.AddAssemblyAIAudioToText(
            apiKey: ApiKey,
            endpoint: new Uri(Endpoint),
            serviceId: ServiceId
        );
        using var provider = services.BuildServiceProvider();

        // Assert
        var service = provider.GetRequiredKeyedService<IAudioToTextService>(ServiceId);
        Assert.NotNull(service);
        Assert.IsType<AssemblyAIAudioToTextService>(service);
    }
}