# プログラミング言語LP

## 文法(syntax)

### データ型

```
1 // 整数
-1 // マイナス
1.5 // 小数
true // bool
false
"文字列" // 文字列
[1,1.5,true,"string"] // 配列
{ :a => 1, false => "hoge" } // ハッシュ
```

## 演算子

```
1 + 2
3 - 2
3 * 4
10 / 2
5 % 2; // 剰余和
5 ** 6; // 冪乗
```

### if文

```
if(true)
  true
else
  false
end
```

### case文

```
case 1
when 2
  "2"
when true
  "true"
else
  "false"
end
```

### 関数定義

```
def hoge(a, b=1, *c, &d)
   print(a)
end
```
