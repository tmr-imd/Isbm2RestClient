﻿namespace BlazorServerExample.Data;

public record class StructureAssetsFilter
(
    string FilterCode,
    string FilterType,
    string FilterLocation,
    string FilterOwner,
    string FilterCondition,
    string FilterInspector
);
