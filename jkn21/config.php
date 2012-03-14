<?php
/*
 * config.php
 * JKDesktopSearch Proxy Server CGI config file
 *
 * Powerd by BUKKYO UNIVERSITY LIBRARY Assit by CMS
 *
 */

// JapanKnowledgeとの契約キー
define("CONTRACT_KEY", "XXXXXXXXXX");

// javascript格納フォルダ名(PHPと同じなら省略可)
define("JS_DIR", "");

// CSS格納フォルダ名(PHPと同じなら省略可)
define("CSS_DIR", "");

// イメージ格納フォルダ名※これを変更した場合は各CSSファイル中のurlも変更する必要があります
define("IMG_DIR", CSS_DIR . "/images/");

// プロキシURL
//define("PROXY_URL", "proxy.exsample.ac.jp:8080");

// 検索結果へのスニペット表示を許可するREMOTE_ADDR(正規表現)
define("SNIPPET_DISP_ADDR", "192\\.168\\.1\\.");

// 検索結果へのスニペット表示を許可するREMOTE_HOST(正規表現)
define("SNIPPET_DISP_HOST", "exsample\\.ac\\.jp");
?>
