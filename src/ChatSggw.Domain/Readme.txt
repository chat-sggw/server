Backendowcy, nie wiem do końca jaki jest model tego, jednak
sądzę, że wystarczą dwie klasy do konwersacji, a nawet twierdzę, że dałoby
się zdziedziczyc grupową z bezpośredniej.

Ponadto, nie widzę powodu odrózniać grupowej geo od zwykłej, bo lokalizacja należy do Usera,
nie do konwersacji. Stąd, jeśli geoInfo o userze nie jest null, to mamy geo - wydaje się
bardziej elastyczne i sensowne niż badziewne instancjonowanie danej konwersacji "na chama".

KR