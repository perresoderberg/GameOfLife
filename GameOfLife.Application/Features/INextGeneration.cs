using GameOfLife.Application.Results;
using GameOfLife.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife.Application.Features;

public interface INextGeneration
{
    Result<Grid> Execute(Grid grid);
}
