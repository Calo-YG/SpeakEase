var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SpeakEase>("speakease");

builder.Build().Run();
