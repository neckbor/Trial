﻿using Domain.Entities.Dictionaries;
using Domain.Shared;

namespace Application.Dictionaries.Airlines;

public interface IAirlineService
{
    Task<Result<Airline>> GetByIATACodeAsync(string IATACode);
    Task<Result<Airline>> GetByICAOCodeAsync(string ICAOCode);
}
