# NoWarn — důvody

## CA1416 — platformově specifické API na Windows-only assembly
**Zdroj:** `TargetFramework` je `net10.0-windows7.0`, celá assembly je jen pro Windows, přesto analyzer hlásí CA1416 napříč knihovnou (`WpfApp.cd` apod.).
**Proč nelze opravit bez rizika:** `[SupportedOSPlatform("windows")]` na sdílených třídách se kaskádovitě propaguje na všechny volající projekty a počet warningů násobně roste místo klesá.
**Kdy přehodnotit:** pokud .NET SDK v budoucnu začne CA1416 automaticky potlačovat na základě Windows-specific TFM.

## CS0067 — event je nikdy vyvolán
**Zdroj:** `ImageButtons.Added`, `ErrorListing.ClickCancel`, `WindowWithUserControl.ChangeDialogResultAsync` jsou veřejné eventy bez interního volání.
**Proč nelze opravit bez rizika:** jde o veřejné API NuGet balíčku; smazání eventu by byla breaking change pro odběratele, kteří se na něj mohli přihlásit.
**Kdy přehodnotit:** při major verzi balíčku, kdy je breaking change přípustná.

## CS8600, CS8601, CS8602, CS8604, CS8618, CS8622, CS8625, CS8629 — nullable reference warningy
**Zdroj:** starý WPF kód (extrahovaný z monolitu `SunamoWpf`) s `Nullable=enable` zapnutým dodatečně, stovky call sites.
**Proč nelze opravit plošně:** vyžaduje individuální ověření nullability u stovek míst bez možnosti reálného otestování chování každého okna/controlu.
**Kdy přehodnotit:** při postupné revizi nullability jednotlivých oken.

## CS0108 — člen skrývá zděděný člen bez `new`
**Zdroj:** starý WPF kód, ojedinělé výskyty.
**Proč nelze opravit bez rizika:** vyžaduje ověření, zda jde o záměrné přepsání chování bez `override`/`new`, bez možnosti reálného otestování.
**Kdy přehodnotit:** při postupné revizi jednotlivých oken.
