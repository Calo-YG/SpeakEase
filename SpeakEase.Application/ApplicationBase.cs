﻿using Microsoft.EntityFrameworkCore;

namespace SpeakEase.Application;

public class ApplicationBase<TDbContext>(IServiceProvider serviceProvider, TDbContext dbContext)
      where TDbContext : DbContext
{
      
}