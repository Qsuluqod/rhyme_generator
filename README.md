# Matěj Prokopič

## Slovník rýmů
Funguje jako knihovna pro C#. Pro jednotlivá slova nalezne několik rýmů, k čemuž používá hned několik algoritmů, mezi nimiž je možné vybírat. Mimoděk umí i přepsat text do fonetické transkripce.

## Zadání
Pro česká slova nalezne program v rozumném čase české rýmy. Nalezené rýmy nemusí být nutně lemmata, program nalezne i jiné všemožné slovní tvary. K nalezení rýmů bude používat fonetické a fonologické rysy, podle kterých bude určovat, jak moc jsou si nějaká písmena podobná.

## Zvolený algoritmus
K vyhledávání rýmu je použitý něco jako reverzní strom trie, tedy slova v něm jsou uložena pozpátku. Každé slovo je ve stromě uložené po své fonetické transkripci, například slovo oběd, bychom tedy našli, pokud bychom se vydali od kořene přes fonémy: t, e, j, b, o, protože ve fonetické transkripci je oběd \[objet\]. Ve vrcholu o, ale bude uloženo slovo oběd podle ortografického úzusu jazyka českého (prostě oběd). Z tohoto důvodu také může v jednom vrcholu být více slov. Například ve vrcholu se slovem oběd bude i slovo objet, protože se vysloví stejně. 

Ve vyhledávání rýmů samotném pak slovník nabízí dvě možnosti.

1) Definujeme hloubku rýmu H, tedy kolik posledních písmen se musí rýmovat, aby nás rým uspokojil. Poté vezmeme nějakou funkci (slovník jich nabízí hned několik), která nám pro každý foném dá seznam jemu nejpodobnějších fonémů. Algoritmus je pak jednoduché prohledávání do hloubky trie, kdy z každého vrcholu jdeme do všech vrcholů, které nám dá funkce pro foném aktuálního vrcholu. Toto dělá dokud hloubka hledání nepřesáhla H, potom přejde na ničím neregulované hledání do hloubky, kde si zároveň i v každém vrcholu, kterým projde, zapamatuje slova, která zde jsou, protože to jsou ony hledané rýmy.

2) Možnost s o něco lepšími výsledky (alespoň dle mého lyrického gusta) je hledání za použití fonemické matice podobnosti. To je matice, kde pro každé dva fónemy je zapsána jejich podobnost jako číslo od 0 do 1. Jedna mají pouze dvě totožná písmena. Výsledný algoritmus pak funguje úplně stejně jako ten první, akorát s rozdílem, že si pamatuje pro každý průchod míru rýmování se. Tato míra začíná na čísle 1 a po každém průchodu do dalšího vrcholu se pronásobí mírou podobnosti fonému onoho vrcholu a aktuálního fonému slova, pro které hledáme rým. Uzly, které si tento algoritmus vybírá jsou seřazeny podle míry podobnosti. Pro ujasnění nabízím několik příkladů. <br>
Hledáme rým pro slovo podolí s hloubkou rýmu 4, a prošli jsme fonémy í a l. Nyní se nacházíme v uzlu s fonémem o. Míra rýmování se je zatím 1 (jedná se zatím o dokonalý rým). Algoritmus si nyní vybírá kam půjde dál, nejdříve projde do vrcholu d, protože to je s aktuálním písmenem (které je d), pro které hledáme rým nejpodobnější (je totožné). Po tom, co projde všemi syny vrcholu d (hloubka rýmu je čtyři, tedy všichni synové se již rýmují dostatečně) se algoritmus podívá do vrcholu s fonémem t (t a d jsou si jak z fonologického tak z fonetického hlediska velmi podobné, liší se pouze ve znělosti), řekněme, že míra podobnosti mezi d a t bude 0.9, na tuto hodnotu nám tedy klesne míra rýmu a všichni synové vrcholu s fonémem t budou mít tuto míru rýmu. Tedy například slovo "údolí" má konečnou míru rýmu 1, kdežto slovo "nastolí" má konečnou míru rýmování se pouze 0.9, protože jsme jej našli po cestě z vrcholu t, kdy už se jistá dokonalost rýmu ztratila. <br>
Podle této konečné míry rýmu jsou pak konečné rýmy seřazeny. Pokud v průběhu algoritmu určitá míra rýmu klesne pod určitou hodnotu, hledání do hloubky zde nepokračuje dál a pokračuje se jinde.

## Program
Hlavní řídící struktura programu je statická třída Slovník rýmů (ang.: RhymeDictionary), která i obsahuje funkci Main. Tato třída si vytváří instanci třídy Trie, což je reprezentace obráceného písmenkového stromu. Tato třída umí přidávat do stromu, a vyhledávat ve stromu dvěma způsoby popsanými výše. K tomu první potřebuje zadat jako parametr funkci, podle které bude vyhledávat. K druhé metodě potřebuje zadat matici, podle které bude vyhledávat. Další důležitá třída je IPA (z anglického international fonetic alphabet). Tato třída je rovněž statická a obsahuje velké množství fonetických a fonologických informací o písmenech. Slovník ji volá když potřebuje udělat fonetickou transkripci nebo když potřebuje sestavit matici podobnosti. Dále tato třída obsahuje všechny funkce, které se dají použít k vyhledávání rýmů podle první metody a zároveň i všechny matice, které se dají použít podle metody druhé. Další třídy, které funguje vesměs pouze jako přehledné úschovny dat jsou: vrchol (ang.: Node, jeden vrchol stromu trie), fonemická matice (ang.: PhonemMatrix ona matice podobnosti), pochod (ang.: March, reprezentace jednoho průchodu při vyhledávání ve stromu trie) a rým (ang.: Rhyme, reprezentace jednoho nalezeného rýmu). 

Průběh programu by tedy šel popsat nějak následovně. Slovník rýmu si vytvoří instanci Trie, načež zavolá metodu stromu Trie, která strom Trie vytoří ze slov uloženém v nějakém csv. Poté slovník rýmu zavolá třídu IPA, aby sestavila matice podobnosti, z dat které v sobě má. Nakonec Slovník rýmu zavolá metody ve funkci Trie a jako atributy předá matice podobnosti nebo funkce podobnosti třídy IPA, vrácené rýmy pak slovník rýmu sám vypíše.

## Alternativní programová řešení
Původně jsem chtěl úlohu řešit tak, že bych vytvořil Trii, kam bych ale každé slovo uložil vícekrát, podle jeho fonetických nebo fonologických vlastností. Tedy nebyly by zde pouze vrcholy pro fonémy, ale i vrcholy pro nasály, frikativy, stridentní atd. Každé slovo by ale tímto způsobem muselo být zapsáno fakt hodně způsoby a Trie by tak hodně narostla, což by ve výsledku přineslo hodně problémů s pamětí i časem, v jakém by se v trii něco hledalo. Proto jsem se rozhodl pro svoje nynější řešení úlohy.

## Reprezentace vstupních dat a jejich příprava
Vstupní data existují pro můj program dvě. První je obrovské množství slov, ze kterých se postaví Trie a ze kterých pak hledáme rýmy. Tato slova by měla být v jednom souboru oddělená nějakým symbolem (ideálně čárkou nebo středníkem) bez zbytečných mezer okolo. V souboru mohou být jen česká slova (nebo slova která jdou zapsat českým ortografickým inventářem), speciální písmena jako ä, ö, ô, ê, nebo znaky jako -, _, : způsboí rozbití programu. Jako vstupní slovo pro hledání rýmů musí být string, podle stejných pravidel, jako pro soubor, ze kterého se staví trie. Slova nemusí být zapsaná fonetickou transkripcí, ale pokud budou, nebude to ničemu vadit.

## Reprezentace výstupních dat a jejich interpretace
Jako výstupní data dostane uživatel převážne pole rýmů. V případě druhé vyhledávací metody budou i seřazená podle míry rýmování.

## Průběh
Nejtěžší na práci bylo vymyslet, jak vlastně určím jak moc jsou si některá písmena podobná. Fonetická pravidla dopadla vesměs katastrofálně a tak jsem se přesunul na ta fonologická, která už fungovala docela slušně. Důležité bylo posílit vazby mězi znělostními páry, aby byla jistota, že první foném, do kterého zkusí vyhledávání jít bude po tomtéž fonému jeho znělostní alternativa (s-z, t-d).

## Možná rozšíření
Fonetická transkripce není, musím přiznat, úplně dotažena do konce. Chybí především dvě věci, přidání rázu na začátku slov, co začínají na vokál a zpodoba znělosti u shluku obstruentů. Tyto jevy se ale vyskytují téměř výhradně na začátku slov, pročež pro účely hledání rýmu nejsou tyto jevy zas tak důležité a bylo by možné je zanedbat. Co dále není dokonalé je matice podobnosti fonémů. U té by bylo nejlepší vyplnit vztah pro každá dvě písmena ručně, podle nějakého laboratorního měření. To ale už bohužel není v mých silách.

## Závěr
Věřím, že nástroj, který jsem vytvořil funguje úctyhodně, ale chybí mu správná data podbností jednotlivých fonémů. Kdyby tato data byla doplněna, věřím že vyhledávání pomocí matice podobností by bylo fenomenální. Další věc co programu chybí, je dostatečně velká knihovna, ve kterých se rýmy hledají. Nebyl jsem ale, schopný najít nějakou větší databázi slov. Zároveň je dobře, že slova, ze kterých se hledají rýmy, jsou ta která, alespoň Karel Čapek nejčastěji používal a né nějaké okrajové termíny. Tudíž se s mojí knihovnou dá tvořit lidová poezie proletářského rázu.

## Zdroje
Seznam slov ze kterých aplikace hledá rýmu je sebrán z Českého národního korpusu. Konkrétně se jedná a deset tisíc nejpoužívanějších slovních tvarů, která ve všech svých dílech používal Karel Čapek. Pro vyplnění fonetických a fonologických rysů jednotlivých fonémů jsem použil hlavně svoje zápisky z hodin Fonetiky a Fonologie, pročež bych zde chtěl poděkovat i Magdaleně Králové Zíkové, která mi dala znalosti potřebné k sestavení této práce.
