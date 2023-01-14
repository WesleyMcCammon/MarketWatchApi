using Skender.Stock.Indicators;

public class PivotPoint
{
    public string Symbol { get; set; } = string.Empty;
    public string PivotName { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public PivotPointsResult PivotPointsResult { get; set; } = default!;

    public PivotPoint(string symbol, string pivotName, PivotPointsResult pivotPointsResult)
    {
        Symbol = symbol;
        PivotName = pivotName;
        DateTime = pivotPointsResult.Date;
        PivotPointsResult = pivotPointsResult;
        if (PivotPointsResult.R4 != null) PivotPointsResult.R4 = Format(PivotPointsResult.R4);
        if (PivotPointsResult.R3 != null) PivotPointsResult.R3 = Format(PivotPointsResult.R3);
        if (PivotPointsResult.R2 != null) PivotPointsResult.R2 = Format(PivotPointsResult.R2);
        if (PivotPointsResult.R1 != null) PivotPointsResult.R1 = Format(PivotPointsResult.R1);
        if (PivotPointsResult.PP != null) PivotPointsResult.PP = Format(PivotPointsResult.PP);
        if (PivotPointsResult.S1 != null) PivotPointsResult.S1 = Format(PivotPointsResult.S1);
        if (PivotPointsResult.S2 != null) PivotPointsResult.S2 = Format(PivotPointsResult.S2);
        if (PivotPointsResult.S3 != null) PivotPointsResult.S3 = Format(PivotPointsResult.S3);
        if (PivotPointsResult.S4 != null) PivotPointsResult.S4 = Format(PivotPointsResult.S4);
    }

    private decimal Format(decimal? decimalValue)
    {
        int rounding = Symbol.Contains("JPY") ? 2 : 5;
        return decimalValue.HasValue ? Math.Round(decimalValue.Value, rounding) : 0;
    }
}