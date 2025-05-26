var builder = DistributedApplication.CreateBuilder(args);


builder.AddProject<Projects.SpeakEase_Socail>("speakease-socail");

builder.AddProject<Projects.SpeakEase_Gateway>("speakease-gateway");

builder.AddProject<Projects.SpeakEase_Study_Host>("speakease-study-host");

builder.Build().Run();
