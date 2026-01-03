# Statistica ruznych casti hry
## Fps

## Worl Load
| Version | World (15x15) | %      |
| ------- | -------------:| ------ |
| 1.0     | 5.951s        | +0%    |
| 1.1     | 1.819s        | -69.4% |
| 1.3     | 1.131s        | -37.8% |
| 2.0     | 1.519s        | +34.3% |

> [!NOTE]
> - Procenta jsou relativni k predhozi verzi
> - Cim mensi cas tim lepsi

## World Generation
| Version | Chunk (first) | %      | Chunk (avg) | %      | World (15x15) | %      |
| ------- | -------------:| ------ | -----------:| ------ | -------------:| ------ |
| 1.0     | 3.47ms        | +0%    | 0.43ms      | +0%    | ...           | ...    |
| 1.1     | 1.88ms        | -45.8% | 0.39ms      | -9.3%  | 600.4ms       | +0%    |
| 1.3     | 1.93ms        | +2.7%  | 0.48ms      | +23.1% | 621ms         | +3.4%  |

| Version | Chunk (first) | %      | Chunk (avg) | %      | World (17x17) | %      |
| ------- | -------------:| ------ | -----------:| ------ | -------------:| ------ |
| 2.0     | 4.80ms        | +148.7%| 1.19ms      | +147.9%| 868ms         | +39.8% |

> [!NOTE]
> - Od verze 2.0 se generuje 17x17 chunku
> - Procenta jsou relativni k predhozi verzi
> - Cim mensi cas tim lepsi

## World Render
| Version | Chunk (first) | %      | Chunk (avg) | %      | World (15x15) | %      |
| ------- | -------------:| ------ | -----------:| ------ | -------------:| ------ |
| 1.0     | 915.28ms      | +0%    | 3070.29ms   | +0%    | ...           | ...    |
| 1.1     | 16.25ms       | -98.2% | 5.33ms      | -99.8% | 1.219s        | +0%    |
| 1.3     | 13.33ms       | -18.0% | 2.20ms      | -58.7% | 0.509s        | -58.2% |
| 2.0     | 17.95ms       | +34.7% | 2.80ms      | +27.3% | 0.651s        | +27.9% |

> [!NOTE]
> - Procenta jsou relativni k predhozi verzi
> - Cim mensi cas tim lepsi
