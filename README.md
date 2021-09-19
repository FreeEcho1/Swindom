# Swindom

[開発状況]<br>
開発の継続が難しい状況なので、とりあえずオープンソースとして公開しました。<br>
もし誰かが派生ソフトウェアとして開発してくれるのであれば、実現できていない要望があるのでお願いできればと思っています。<br>
・スクリプトでウィンドウを処理する機能。<br>
・仮想デスクトップの対応。<br>
・「処理しない条件」の「サイズ」を指定していて、ソフトウェアのアップデートでUI (ウィンドウサイズ) が変更された場合に処理されてしまうので、何らかの対策。<br>
<br>
[ソフトウェアのダウンロード方法]<br>
右側の「Release」からダウンロードできます。<br>
ファイル名に「no_dpi」が入っているファイルは、DPI非対応版です。<br>
DPIに対応させるとヘルプファイル(hh.exe)のウィンドウで不具合が発生するので、<br>
ヘルプファイルのウィンドウを処理させたい場合は、DPI非対応版を使用してください。<br>
「Source code」はソースコードです。ソフトウェアではありません。<br>
<br>
[説明]<br>
ソースコードに関してですが、時間の関係で整理できてなかったりするので、良くない書き方だったり綺麗ではない部分があります。<br>
<br>
[ソフトウェアの説明]<br>
ウィンドウの位置やサイズを変更するなど、ウィンドウに関する処理を自動及びホットキーで処理することができます。<br>
<br>
[情報]<br>
制作者 : FreeEcho<br>
ソフトウェア名 : Swindom<br>
フリーソフトウェア (商用利用可能)<br>
<br>
[動作環境]<br>
Windows 10 (64 bit)<br>
「.NET 5」に対応しているOSであれば動作します。<br>
32 bit OSは未確認ですが多分動作します。<br>
64 bit OSは64 bitとして動作、32 bit OSは32 bitとして動作します。<br>
<br>
[必要なもの]<br>
 - .NET 5<br>
「.NET 5」がインストールされていない場合はインストールしてください。<br>
<br>
[機能]<br>
 - ウィンドウの位置やサイズの変更<br>
 - ウィンドウの移動中に画面端や別のウィンドウに貼り付ける<br>
 - ウィンドウの最前面化<br>
 - ウィンドウの透明化<br>
 - ホットキーによる操作<br>
 - ウィンドウが表示された時に画面の中央に移動<br>
 - 画面外にあるウィンドウを画面内に移動<br>
 - マルチディスプレイに対応<br>
<br>
[DLLのソースコード]<br>
FreeEcho Check For Update<br>
https://github.com/FreeEcho1/FreeEcho-Check-For-Update<br>
FreeEcho Window Move Detection Mouse<br>
https://github.com/FreeEcho1/FreeEcho-Window-Move-Detection-Mouse<br>
FreeEcho Window Selection Mouse<br>
https://github.com/FreeEcho1/FreeEcho-Window-Selection-Mouse<br>
FreeEcho HotKey WPF<br>
https://github.com/FreeEcho1/FreeEcho-HotKey-WPF<br>
FreeEcho Window Event<br>
https://github.com/FreeEcho1/FreeEcho-Window-Event<br>
ControlsStyle<br>
https://github.com/FreeEcho1/ControlsStyle<br>
FreeEcho Controls<br>
https://github.com/FreeEcho1/FreeEcho-Controls<br>
SwindomImage<br>
https://github.com/FreeEcho1/SwindomImage<br>
SwindomTaskScheduler<br>
https://github.com/FreeEcho1/Swindom-TaskScheduler<br>
