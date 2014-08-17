McCommandStudio
===

Minecraft のコマンドの記述に特化した IDE ライクなテキストエディタです。  
主にコマンドブロックなどで使用するような長く複雑なコマンドの記述を得意とします。  
コマンドを自分で記述できる (コマンドの知識がある) 方を対象としています。  

正確にはコマンドの形式とは違う独自フォーマット (後述) で行うことになります。  
あくまでテキストエディタベースなので、いわゆるジェネレータ的な機能 (GUI で値を選択できる機能) は備えていません。  

今後ほかにも新しい機能が追加される可能性があります。逆もあるかもしれません。  
現在 ALPHA 版です。限定的に公開しています。

コマンドエディタの機能
--
* シンタックスハイライト (特定のキーワードに色が付く)
* 行番号、半角スペース、タブなどの表示
* 改行に対応
* コメントアウト (//, /**/) に対応
* 複数行にわたる括弧やコメントの折りたたみ
* 1 コマンドにつき 1 テキストファイルベースでの管理  
複数コマンド対応予定です
* ドキュメントタブのドッキング、フローティング
* リアルタイムなコマンド生成とコピー
* 自動エスケープ  
クォーテーションのエスケープ方法の新旧対応 ([14w31a[MC-37661]](https://bugs.mojang.com/browse/MC-37661))

#####独自フォーマット
…といってもほぼコマンドと同じの形式ですが、下記の特徴があります。  
これにより、シンタックスハイライトを活かしたまま記述ができます。  

* ダブルクォーテーションで囲む必要のある特定のデータタグの値をダブルクォーテーションなしで直接記述できる  
看板の Text1 などにデータタグを指定する際、全体をクォーテーションで囲んで中身をエスケープする必要がありません
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
* 文字列をダブルクォーテーションではなく ( ) で囲んで記述できる  
ただし ( ) の中身は通常の文字列ではなくトークンとして処理されるため、通常の文字列での使用は推奨しません  
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
開発者は Windows 8.1 で動作確認を行っています。  
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
* [WPF Toolkit](https://www.nuget.org/packages/WPFToolkit/)
* [Extended WPF Toolkit](https://wpftoolkit.codeplex.com/)
* [WPF File System Controls](http://fsc.codeplex.com/)

また下記のアプリケーションの一部ソースコードを参考にさせていただきました。

* [Krile StaryEyes](http://krile.starwing.net/) (Tokenizer, QueryEditor, etc)
* [提督業も忙しい！](http://grabacr.net/kancolleviewer) (Settings, Xml-related)
* [Edi](http://edi.codeplex.com/) (AppCommands, how to use AvalonEdit and AvalonDock)

この場を借りて様々な有益なライブラリやソースコードを公開してくださっている方々に感謝いたします。

ライセンス
--
[MIT License](http://opensource.org/licenses/mit-license.php) の下で公開しています。  

Copyright © 2014 [yuri (Cafemoca)](https://twitter.com/yuri_v3v)  
