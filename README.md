Minecraft Command Studio
========================

Minecraft のコマンドの記述に特化した IDE ライクなテキストエディタです。  
主にコマンドブロックなどで使用するような長く複雑なコマンドの記述を得意とします。  
あくまでテキストエディタなので、コマンドを自分で記述できる方を対象としています。  
いわゆるジェネレータ的な機能 (GUI で値を選択できる機能) は備えていません。  

正確には、コマンドの形式とは違う独自フォーマット (後述) で記述します。

機能・特徴
--
* シンタックスハイライト (特定のキーワードに色が付く)
* コンプリーション (入力補完) ウィンドウ 
現在、データタグの補完は未対応です
* 行番号、半角スペース、タブなどの可視化
* 改行に対応
* コメントアウト (//, /**/) に対応
* 複数行にわたる括弧やコメントの折りたたみ
* 1 コマンドにつき 1 テキストファイルベースでの管理  
今後、複数コマンド対応予定です
* ドキュメントタブのドッキング、フローティング
* リアルタイムなコマンド生成とコピー
* 自動エスケープ (後述)  
クォーテーションのエスケープ方法の新旧対応 ([14w31a[MC-37661]](https://bugs.mojang.com/browse/MC-37661))

#####※ 独自フォーマット
…といってもほぼコマンドと同じ形式ですが、下記の特徴があります。  
これにより、シンタックスハイライトを活かしたまま記述ができます。  

* データタグ内で Raw JSON Text を記述する際、ダブルクォーテーションなしで直接記述できる  
本来は中身をダブルクォーテーションで囲む必要がありますが、その必要がありません  
このとき必要なダブルクォーテーション、およびそれがネストする場合に必要なエスケープシーケンスは自動的に付加されます
``` text
/setblock ~ ~1 ~ minecraft:standing_sign 0 replace {
	Text1: {
		text: "看板",
		clickEvent: {
			action: run_command,
			value: "/tellraw @p メッセージ"
		}
	}
}
```
* データタグ内で文字列としてコマンドを記述する際、 ( ) で囲んで記述できる  
これを利用することで、シンタックスハイライトを活かした記述が可能です  
括弧の中身はトークンとして処理されるため、通常の文字列での使用は推奨しません  
clickEvent の value 値など、コマンド文の文字列を書く際 (特に改行したい場合) に有効です
``` text
/setblock ~ ~1 ~ minecraft:standing_sign 0 replace {
	Text1: {
		text: "看板",
		clickEvent: {
			action: run_command,
			value: (
				/blockdata ~ ~ ~ {
					Text1: "クリックされた"
				}
			)
		}
	}
}
```

動作条件・環境
--
* Windows 7 以降
* [.NET Framework 4.5](http://www.microsoft.com/ja-jp/download/details.aspx?id=30653)

Windows 8 以降をお使いの場合 .NET Framework 4.5 は標準でインストールされています。  
開発者は Windows 10 で動作確認を行っています。  
何らかの不具合が起きた場合、お使いの環境を添えてご報告いただけると嬉しいです。  

使用ライブラリ
--
下記のライブラリを使用しています。  

* [Reactive Extensions](http://rx.codeplex.com/)
* Interactive Extensions
* [Reactive Property](http://reactiveproperty.codeplex.com/)
* [Livet](http://ugaya40.net/livet)
* [MahApps.Metro](http://mahapps.com/)
* [AvalonEdit](http://avalonedit.net/)
* [AvalonDock](https://avalondock.codeplex.com/)
* [Windows API Code Pack](http://archive.msdn.microsoft.com/WindowsAPICodePack)
* [WPF TaskDialog](http://www.codeproject.com/Articles/137552/WPF-TaskDialog-Wrapper-and-Emulator)
* [WPF Toolkit](https://www.nuget.org/packages/WPFToolkit/)
* [Extended WPF Toolkit](https://wpftoolkit.codeplex.com/)
* [ReadJEnc](http://www.vector.co.jp/soft/winnt/util/se506899.html)

また下記のアプリケーションの一部ソースコードを参考にさせていただきました。

* [Krile StaryEyes](http://krile.starwing.net/) (Tokenizer, QueryEditor, SweetMagic, etc)
* [提督業も忙しい！](http://grabacr.net/kancolleviewer) (Settings, Xml-related)

この場を借りて様々な有益なライブラリやソースコードを公開してくださっている方々に感謝いたします。

ライセンス
--
[MIT License](http://opensource.org/licenses/mit-license.php) の下で公開しています。  

Copyright © 2014-2015 [yuri (cafemoca)](https://twitter.com/yuri_v3v)  
