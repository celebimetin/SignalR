select tarih,[1],[2],[3],[4],[5] 
from (select [City],[Count],
Cast([CovidDate] as date) as tarih from Covids) as covidTable
PIVOT
(sum(count) for City IN([1],[2],[3],[4],[5])) as pTable
order by tarih asc