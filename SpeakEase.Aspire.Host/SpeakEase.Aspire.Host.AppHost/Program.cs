var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SpeakEase>("speakease");

builder.AddProject<Projects.SpeakEase_Socail>("speakease-socail");

builder.AddProject<Projects.SpeakEase_Gateway>("speakease-gateway");

builder.Build().Run();
