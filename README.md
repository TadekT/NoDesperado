# NoDesperado

Gra skradankowa inspirowana Desperados 3, stworzona w Unity. Gracz steruje postacią z perspektywy izometrycznej, unikając przeciwników i zbierając klucze potrzebne do ukończenia poziomu.

## Rozgrywka

- Sterowanie kliknięciem myszy (point-and-click) — gracz porusza się klikając w wybrany punkt
- Zbierz klucz, a następnie dotrzyj do wyjścia z poziomu
- Ukryj się w krzakach, aby stać się niewidocznym dla przeciwników
- Unikaj lub oszukaj przeciwników, którzy mają rozbudowane stany AI

## Sterowanie

| Akcja | Przycisk |
|---|---|
| Ruch postaci | LPM (klik w cel) |
| Podgląd stożka widzenia przeciwnika | ALT + LPM (klik na przeciwnika) |

## AI Przeciwnika

Każdy przeciwnik (Enemy3) posiada maszynę stanów z następującymi stanami:

| Stan | Opis |
|---|---|
| **Patrol** | Porusza się między waypointami |
| **Idle** | Stoi w miejscu przez chwilę po dotarciu do waypointa |
| **Suspicious** | Widzi gracza — zatrzymuje się i obserwuje |
| **Chase** | Goni gracza biegnąc |
| **Attack** | Gracz w zasięgu — strzela |
| **Search** | Stracił gracza z oczu — przeszukuje okolicę |

Przeciwnik traci gracza z pola widzenia jeśli gracz schowa się w krzakach.

## Podgląd pola widzenia (FOV)

Przytrzymaj **ALT** i kliknij LPM na wybranego przeciwnika, aby przełączyć widoczność jego stożka widzenia. Każdy przeciwnik działa niezależnie — można podejrzeć wielu naraz.

Kolor stożka informuje o stanie przeciwnika:
- Zielony — spokojny (Patrol / Idle)
- Pomarańczowy — podejrzliwy (Suspicious / Search)
- Czerwony — alarm (Chase / Attack)

## Technologie

- Unity 6
- Unity Input System (nowy)
- NavMesh (poruszanie AI i gracza)
- TextMeshPro
- URP (Universal Render Pipeline)

## Struktura projektu

```
Assets/
├── Input/              # InputActions asset
├── Prefab/             # Prefaby przeciwników, gracza, obiektów
└── Script/
    ├── AI_Enemy/
    │   ├── Enemy3/     # Aktywny typ przeciwnika (Brain, Vision, Combat, Movement, FOV)
    │   └── Enemy1&2/   # Starsze typy (nieużywane)
    └── UI/             # HealthBarUI, FaceCamera
```
