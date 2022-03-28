Select 
    Max(Score) as TopScore,
    AVG(Score ) as Average_Score, 
    Count(SessionCount) as GameCount,
    Sum(ItemCount) as EarnedItemCount
    Sum(PurchasedItemCount) as PurchasedItemCount
    SUM(CoinBalance) as Balance
    Sum DollarSpent as RealCurrencySpent
    
{
    DataSource = $"projects/{projectId}/instances/{instanceId}/databases/{databaseId}";
    EmulatorDetection = "EmulatorOnly"