<?php
/*
 *	desktop.php
 *  JKDesktopSearch Proxy Server CGI
 *
 *  Powerd by BUKKYO UNIVERSITY LIBRARY Assit by CMS
 *
 */
?>
<?php
	include('config.php');
?>
<?php
$searchWord = !empty($_GET['searchWord']) ? htmlspecialchars($_GET['searchWord'], ENT_QUOTES) : "";
$contentsSelect = htmlspecialchars($_GET['contentsSelect'], ENT_QUOTES);
$currentPage = htmlspecialchars($_GET['currentPage'], ENT_QUOTES);
$flowMode = htmlspecialchars($_GET['flowMode'], ENT_QUOTES);

//google complete API
function complete($searchWord,$contentsSelect,$flowMode,$className){
if (defined("PROXY_URL")) {
	$proxy_opts = array(
	'http' => array(
	'proxy' => 'tcp://' . PROXY_URL ,
	'request_fulluri' => true,
	),
	);
	$proxy_context=stream_context_create($proxy_opts);
} else {
	$proxy_context=null;
}
$raw = file_get_contents("http://www.google.com/complete/search?hl=jn&q=".$searchWord."&output=toolbar",false,$proxy_context);

$xml = simplexml_load_string($raw);

$hits = $xml->CompleteSuggestion;
if($xml->CompleteSuggestion){
echo "<div id='no_result' class='".$className."'><h3>もしかして…</h3><ul>";
foreach ($hits as $hit) {
$suggestion = $hit->suggestion;
$data = $suggestion['data'];
echo '<li>';
echo "<a href='desktop.php?searchWord=".$data."&currentPage=1&contentsSelect=".$contentsSelect."&flowMode=".$flowMode."'>".$data."</a>";
echo '</li>';
}
echo "</ul></div>";

}
}
?>
<!DOCTYPE html>
<html dir="ltr" lang="ja">
<head>
<meta charset="utf-8" />
<meta name="robots" content="nofollow">
<title><?php if(!empty($_GET['searchWord'])){echo $searchWord." - ";} ?>ジャパンナレッジ | 佛教大学図書館ポータルサイト</title>
<?php
	echo("<link rel=\"stylesheet\" media=\"screen,tv\" href=\"" . CSS_DIR . "desktop.css\" />");
?>

<?php if(!empty($_GET['searchWord'])){echo "<link href=\"" . CSS_DIR . "jquery.mCustomScrollbar.css\" rel=\"stylesheet\" media=\"screen,tv\" type=\"text/css\" /> ";} ?>
<?php
echo("<link rel=\"stylesheet\" href=\"" . CSS_DIR . "print.css\" type=\"text/css\" media=\"print\" />");
?>
<!--[if lt IE 9]>
<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
<script type="text/javascript" src="http://www.google.com/jsapi"></script>
<script type="text/javascript">
	google.load("jquery", "1");
	google.load("jqueryui", "1.8.5");

function OnLoad() {

//パラメータ取得
function getUrlVars(){
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for(var i = 0; i <hashes.length; i++)
    	{
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    	}
    return vars;
};

var searchWord = getUrlVars()["searchWord"];
var flowMode = getUrlVars()["flowMode"];

function formSet(searchWord,flowMode) {
$("#jkn21_text").val(decodeURI(searchWord));

	if(flowMode=="1"){
		$("#flowMode2").attr('checked', false);
		$("#flowMode3").attr('checked', false);		
		$("#flowMode1").attr('checked', "checked");	
	}else if(flowMode=="2"){
		$("#flowMode1").attr('checked', false);
		$("#flowMode3").attr('checked', false);		
		$("#flowMode2").attr('checked', "checked");	
	}else if(flowMode=="3"){
		$("#flowMode1").attr('checked', false);
		$("#flowMode2").attr('checked', false);		
		$("#flowMode3").attr('checked', "checked");	
	}
}

if(searchWord || searchWord=="undefined"){
	formSet(searchWord,flowMode);
}

$("<div id='jkn21_detail_select' class='hide'></div>").appendTo(".iform");

$("#jkn21_search_detail").live("click",function(){
$("#jkn21_detail_select").slideToggle("slow");
$("#jkn21_search_detail").toggleClass("close_detail");
$("#jkn21_search_detail").toggleClass("open_detail");
$(".open_detail").text("詳細");
$(".close_detail").text("閉じる");
});

var title_list_dictionary = {
"日本大百科全書(ニッポニカ)":"10010",
"ニッポニカ・プラス":"10011",
"日本国語大辞典":"20020",
"デジタル大辞泉":"20010",
"字通":"20030",
"数え方の辞典":"20040",
"国史大辞典":"30010",
"日本歴史地名大系":"30020",
"誰でも読める日本史年表":"30030",
"江戸名所図会":"30100",
"日本人名大辞典":"50110",
"JK Who's Who":"50120",
"ランダムハウス英和大辞典":"40010",
"e-プログレッシブ英和中辞典":"40020",
"プログレッシブ和英中辞典":"40030",
"Encyclopedia of Japan":"40050",
"コウビルド米語版英英和辞典":"40061",
"CAMBRIDGE英英辞典":"40070",
"理化学英和辞典":"40100",
"医学英和辞典":"40110",
"ポケプロ独和辞典":"42150",
"ポケプロ仏和辞典":"42250",
"ポケプロ西和辞典":"42350",
"ポケプロ伊和辞典":"42450",
"羅和辞典":"42550",
"ポケプロ和独辞典":"42151",
"ポケプロ和仏辞典":"42251",
"ポケプロ和西辞典":"42351",
"ポケプロ和伊辞典":"42451",
"和羅辞典":"42551",
"情報・知識　imidas":"50010",
"現代用語の基礎知識":"50020",
"亀井肇の新語探検":"50310",
"会社四季報（2011年2集春号）":"50410",
"世界文学大事典":"51010",
"デジタル化学辞典(第2版)":"58060",
"法則の辞典":"58110"
}

var title_list_things = {
"日本大百科全書(ニッポニカ)":"10010",
"ニッポニカ・プラス":"10011",
"Encyclopedia of Japan":"40050",
"国史大辞典":"30010",
"情報・知識　imidas":"50010",
"現代用語の基礎知識":"50020",
"デジタル大辞泉":"20010",
"世界文学大事典":"51010",
"デジタル化学辞典(第2版)":"58060",
"法則の辞典":"58110"
}

var title_list_word = {
"日本国語大辞典":"20020",
"デジタル大辞泉":"20010",
"字通":"20030",
"数え方の辞典":"20040"
}

var title_list_english = {
"ランダムハウス英和大辞典":"40010",
"e-プログレッシブ英和中辞典":"40020",
"プログレッシブ和英中辞典":"40030",
"Encyclopedia of Japan":"40050",
"コウビルド米語版英英和辞典":"40061",
"CAMBRIDGE英英辞典":"40070",
"理化学英和辞典":"40100",
"医学英和辞典":"40110"
}

var title_list_foreign_language = {
"ポケプロ独和辞典":"42150",
"ポケプロ和独辞典":"42151",
"ポケプロ和仏辞典":"42251",
"ポケプロ仏和辞典":"42250",
"ポケプロ西和辞典":"42350",
"ポケプロ和西辞典":"42351",
"ポケプロ伊和辞典":"42450",
"ポケプロ和伊辞典":"42451",
"羅和辞典":"42550",
"和羅辞典":"42551"
}

var title_list_news_word = {
"情報・知識　imidas":"50010",
"現代用語の基礎知識":"50020",
"デジタル大辞泉":"20010",
"亀井肇の新語探検":"50310"
}

var title_list_news = {
"JK Who's Who":"50120",
"デジタル大辞泉":"20010",
"亀井肇の新語探検":"50310"
}

var title_list_company = {
"会社四季報（2011年2集春号）":"50410",
"日本大百科全書(ニッポニカ)":"10010"
}

var title_list_person = {
"日本人名大辞典":"50110",
"JK Who's Who":"50120",
"日本大百科全書(ニッポニカ)":"10010",
"デジタル大辞泉":"20010",
"世界文学大事典":"51010"
}

var title_list_history = {
"国史大辞典":"30010",
"誰でも読める日本史年表":"30030",
"日本歴史地名大系":"30020",
"日本大百科全書(ニッポニカ)":"10010",
"日本国語大辞典":"20020",
"デジタル大辞泉":"20010",
"世界文学大事典":"51010",
"江戸名所図会":"30100"
}

var title_list_place = {
"日本歴史地名大系":"30020",
"日本大百科全書(ニッポニカ)":"10010",
"デジタル大辞泉":"20010",
"日本国語大辞典":"20020",
"国史大辞典":"30010",
"江戸名所図会":"30100"
}

var title_list_story = {
"亀井肇の新語探検":"50310",	
"JK Who's Who":"50120"
}

var dictionary_list = "";
var dictionary_list_things ="";
var dictionary_list_word ="";
var dictionary_list_english ="";
var dictionary_list_foreign_language ="";
var dictionary_list_news_word ="";
var dictionary_list_news ="";
var dictionary_list_company ="";
var dictionary_list_person ="";
var dictionary_list_history ="";
var dictionary_list_place ="";
var dictionary_list_story ="";


for (var key in title_list_dictionary) {
	dictionary_list += "<li data-jkn21-titleid='" + title_list_dictionary[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_dictionary[key] + "' id='title_" + title_list_dictionary[key] + "' /><label for='title_" + title_list_dictionary[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_dictionary[key] +"'></div></li>";
}


for (var key in title_list_things) {
	dictionary_list_things += "<li data-jkn21-titleid='" + title_list_things[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_things[key] + "' id='title_" + title_list_things[key] + "' /><label for='title_" + title_list_things[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_things[key] +"'></div></li>";
}
for (var key in title_list_word) {
	dictionary_list_word += "<li data-jkn21-titleid='" + title_list_word[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_word[key] + "' id='title_" + title_list_word[key] + "' /><label for='title_" + title_list_word[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_word[key] +"'></div></li>";
}
for (var key in title_list_english) {
	dictionary_list_english += "<li data-jkn21-titleid='" + title_list_english[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_english[key] + "' id='title_" + title_list_english[key] + "' /><label for='title_" + title_list_english[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_english[key] +"'></div></li>";
}
for (var key in title_list_foreign_language) {
	dictionary_list_foreign_language += "<li data-jkn21-titleid='" + title_list_foreign_language[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_foreign_language[key] + "' id='title_" + title_list_foreign_language[key] + "' /><label for='title_" + title_list_foreign_language[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_foreign_language[key] +"'></div></li>";
}
for (var key in title_list_news_word) {
	dictionary_list_news_word += "<li data-jkn21-titleid='" + title_list_news_word[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_news_word[key] + "' id='title_" + title_list_news_word[key] + "' /><label for='title_" + title_list_news_word[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_news_word[key] +"'></div></li>";
}
for (var key in title_list_news) {
	dictionary_list_news += "<li data-jkn21-titleid='" + title_list_news[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_news[key] + "' id='title_" + title_list_news[key] + "' /><label for='title_" + title_list_news[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_news[key] +"'></div></li>";
}
for (var key in title_list_company) {
	dictionary_list_company += "<li data-jkn21-titleid='" + title_list_company[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_company[key] + "' id='title_" + title_list_company[key] + "' /><label for='title_" + title_list_company[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_company[key] +"'></div></li>";
}
for (var key in title_list_person) {
	dictionary_list_person += "<li data-jkn21-titleid='" + title_list_person[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_person[key] + "' id='title_" + title_list_person[key] + "' /><label for='title_" + title_list_person[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_person[key] +"'></div></li>";
}
for (var key in title_list_history) {
	dictionary_list_history += "<li data-jkn21-titleid='" + title_list_history[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_history[key] + "' id='title_" + title_list_history[key] + "' /><label for='title_" + title_list_history[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_history[key] +"'></div></li>";
}
for (var key in title_list_place) {
	dictionary_list_place += "<li data-jkn21-titleid='" + title_list_place[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_place[key] + "' id='title_" + title_list_place[key] + "' /><label for='title_" + title_list_place[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_place[key] +"'></div></li>";
}
for (var key in title_list_story) {
	dictionary_list_story += "<li data-jkn21-titleid='" + title_list_story[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_story[key] + "' id='title_" + title_list_story[key] + "' /><label for='title_" + title_list_story[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ title_list_story[key] +"'></div></li>";
}


var title_list_article = {
"週刊エコノミスト":"70010",
"日本の論点":"70015",
"NNA：アジア＆EU 国際情報":"70020",
"新編 日本古典文学全集":"80110",
"マルチメディア":"60110",
"東洋文庫":"80010"
}

var article_list_news_word = {
"日本の論点":"70015"
}

var article_list_news = {
"週刊エコノミスト":"70010",
"NNA：アジア＆EU 国際情報":"70020",
"日本の論点":"70015"
}

var article_list_company = {
"週刊エコノミスト":"70010",
"NNA：アジア＆EU 国際情報":"70020"
}

var article_list_person = {
"東洋文庫":"80010"
}

var article_list_history = {
"新編 日本古典文学全集":"80110",	
"東洋文庫":"80010"
}

var article_list_place = {
"東洋文庫":"80010"
}

var article_list_multi = {
"マルチメディア":"60110"
}


var article_list_story = {
"週刊エコノミスト":"70010",
"日本の論点":"70015",
"NNA：アジア＆EU 国際情報":"70020",
"新編 日本古典文学全集":"80110",
"東洋文庫":"80010"
}


var article_news_word ="";
var article_news ="";
var article_company ="";
var article_person ="";
var article_history ="";
var article_place ="";
var article_story ="";
var article_multi ="";


for (var key in article_list_news_word) {
	article_news_word += "<li data-jkn21-titleid='" + article_list_news_word[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + article_list_news_word[key] + "' id='title_" + article_list_news_word[key] + "' /><label for='title_" + article_list_news_word[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ article_list_news_word[key] +"'></div></li>";
}
for (var key in article_list_news) {
	article_news += "<li data-jkn21-titleid='" + article_list_news[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + article_list_news[key] + "' id='title_" + article_list_news[key] + "' /><label for='title_" + article_list_news[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ article_list_news[key] +"'></div></li>";
}
for (var key in article_list_company) {
	article_company += "<li data-jkn21-titleid='" + article_list_company[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + article_list_company[key] + "' id='title_" + article_list_company[key] + "' /><label for='title_" + article_list_company[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ article_list_company[key] +"'></div></li>";
}
for (var key in article_list_person) {
	article_person += "<li data-jkn21-titleid='" + article_list_person[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + article_list_person[key] + "' id='title_" + article_list_person[key] + "' /><label for='title_" + article_list_person[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ article_list_person[key] +"'></div></li>";
}
for (var key in article_list_history) {
	article_history += "<li data-jkn21-titleid='" + article_list_history[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + article_list_history[key] + "' id='title_" + article_list_history[key] + "' /><label for='title_" + article_list_history[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ article_list_history[key] +"'></div></li>";
}
for (var key in article_list_place) {
	article_place += "<li data-jkn21-titleid='" + article_list_place[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + article_list_place[key] + "' id='title_" + article_list_place[key] + "' /><label for='title_" + article_list_place[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ article_list_place[key] +"'></div></li>";
}
for (var key in article_list_multi) {
	article_multi += "<li data-jkn21-titleid='" + article_list_multi[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + article_list_multi[key] + "' id='title_" + article_list_multi[key] + "' /><label for='title_" + article_list_multi[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ article_list_multi[key] +"'></div></li>";
}
for (var key in article_list_story) {
	article_story += "<li data-jkn21-titleid='" + article_list_story[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + article_list_story[key] + "' id='title_" + article_list_story[key] + "' /><label for='title_" + article_list_story[key] + "'>" + key + "</label> <span class='show_explain'></span><div class='title_explain title_"+ article_list_story[key] +"'></div></li>";
}

var article_list = "";
for (var key in title_list_article) {
	article_list += "<li data-jkn21-titleid='" + title_list_article[key] + "'><input class='title_chkbox' type='checkbox' name='titleid' value='" + title_list_article[key] + "' />" + key + " <span class='show_explain'></span><div class='title_explain title_"+ title_list_article[key] +"'></div></li>";
}

$("#list_dictionary h5, #list_article h5, #list_all h5").live("click",function(){
	$(this).next().slideToggle(500);
	$(this).toggleClass("active");
});

		$(".searchForm-act").change(function () {
		if($("#flowMode1").is(":checked")){
		var selectContents = "<div id='list_dictionary'><ul><li class='list_things'><h5>ことがらを調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_things+"</ul></li><li class='list_word'><h5>言葉を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_word+"</ul></li><li class='list_english'><h5>英語を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_english+"</ul></li><li class='list_foreign_language'><h5>外国語を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_foreign_language+"</ul></li><li class='list_news_word'><h5>時事用語を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_news_word+"</ul></li><li class='list_news'><h5>最近の話題を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_news+"</ul></li><li class='list_company'><h5>企業情報を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+
dictionary_list_company+"</ul></li><li class='list_person'><h5>人物を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_person+"</ul></li><li class='list_history'><h5>歴史を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_history+"</ul></li><li class='list_place'><h5>地名・地域を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_place+"</ul></li><li class='list_story'><h5>読み物を楽しむ</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_story+"</ul></li></ul></div>";
		} else if($("#flowMode2").is(":checked")){
		var selectContents = "<div id='list_article'><ul><li class='list_news_word'><h5>時事用語を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+article_news_word+"</ul></li><li class='list_news'><h5>最近の話題を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+article_news+"</ul></li><li class='list_company'><h5>企業情報を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+
article_company+"</ul></li><li class='list_person'><h5>人物を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+article_person+"</ul></li><li class='list_history'><h5>歴史を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+article_history+"</ul></li><li class='list_place'><h5>地名・地域を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+article_place+"</ul></li><li class='list_multi'><h5>マルチメディアを楽しむ</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+article_multi+"</ul></li><li class='list_story'><h5>読み物を楽しむ</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+article_story+"</ul></li></ul></div>";			
		} else if($("#flowMode3").is(":checked")){
		var selectContents = "<div id='list_all'><ul><li class='list_things'><h5>ことがらを調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_things+"</ul></li><li class='list_word'><h5>言葉を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_word + article_news_word+"</ul></li><li class='list_english'><h5>英語を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_english+"</ul></li><li class='list_foreign_language'><h5>外国語を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_foreign_language+"</ul></li><li class='list_news_word'><h5>時事用語を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_news_word+"</ul></li><li class='list_news'><h5>最近の話題を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_news+article_news+"</ul></li><li class='list_company'><h5>企業情報を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+
dictionary_list_company+article_company+"</ul></li><li class='list_person'><h5>人物を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_person+article_person+"</ul></li><li class='list_history'><h5>歴史を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_history+article_history+"</ul></li><li class='list_place'><h5>地名・地域を調べる</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_place+article_place+"</ul></li><li class='list_multi'><h5>マルチメディアを楽しむ</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+article_multi+"</ul></li><li class='list_story'><h5>読み物を楽しむ</h5><ul><li><input type='checkbox' class='checkall' />すべて選択</li>"+dictionary_list_story+article_story+"</ul></li></ul></div>";
		}		
		$("#jkn21_detail_select").html(selectContents);	
		}).change();

		$(".title_chkbox").live("click",function () {
		if($(this).hasClass("checked")){
			$(this).removeClass("checked");
		}else{
			$(this).addClass("checked");
		}			  
		var contentsSelect = [];
		$(":checkbox[name^='titleid']:checked").each(function(){
		contentsSelect.push($(this).val());
		});
		if(contentsSelect.length !== 0) {
		$("#contentsSelect").val(contentsSelect);
		}else{
		$("#contentsSelect").val("all");	
		}
		}).change();		

	$('.checkall').live("click",function () {
		$(this).parent().nextAll().find(':checkbox').attr('checked', this.checked);
		if($(this).parent().nextAll().find(':checkbox').hasClass("checked")){
			$(this).parent().nextAll().find(':checkbox').removeClass('checked');
		}else{
			$(this).parent().nextAll().find(':checkbox').addClass('checked');
		}
	});

	$(".open_explain").live("click",function(){
	$(this).parent().find(".title_explain").slideToggle("slow");
	$(this).toggleClass("show_explain");	
	$(this).toggleClass("open_explain");	
	});

	$(".show_explain").live("click",function(){
	$(".open_explain").parent().find(".title_explain").slideToggle("slow");											 
	$(".open_explain").removeClass("open_explain").addClass("show_explain");
	$(this).toggleClass("show_explain");	
	$(this).toggleClass("open_explain");
	
	$(".title_10010").text("見出し項目約13万項目、総索引語約70万項目という膨大な知識の集大成である『日本大百科全書』のジャパンナレッジ版です。");
	$(".title_10011").text("『日本大百科全書（ニッポニカ）』の補遺版です。テレビ、新聞、インターネットなどで話題になっている時事用語が収録されています。");
	$(".title_20020").text("総項目数50万、用例数100万を収録した国語辞典『日本国語大辞典 第二版』（全13巻）のジャパンナレッジ版です。出典別に用例を網羅的に探し出すなどの機能があります。");
	$(".title_20010").text("現代日本語をはじめ、カタカナ語、古語、専門語、故事・慣用句など22万余りの項目を収録した『大辞泉』のデジタル版です。");
	$(".title_20030").text("白川静著の漢和辞典『字通』のデジタル版です。親字や熟語だけでなく、語義や出典からも項目を検索できます。");
	$(".title_20040").text("ものの名称から数え方を引くことができる、これまでにない画期的な辞典です。");
	$(".title_30010").text("考古・民俗・宗教・美術・国語・国文・地理などを網羅した、日本歴史の全領域をおさめたデジタル歴史大百科です。");
	$(".title_30020").text("地名研究・地域史研究の全成果を集積したデータベースです。 総項目数約20万のデータを完全収録しております。");
	$(".title_30030").text("人名・事項名・歴史用語の検索結果を年代別に表示してくれる事典。年代別や天皇・要職別による検索も可能です。");
	$(".title_30100").text("斎藤幸雄・幸孝・幸成の斎藤家三代の事業として完成された名所案内。東京のみならず近郊の埼玉、千葉などの名所についても記述を残しています。");
	$(".title_50110").text("6万5,000人を超える人名を収録する『日本人名大辞典』は、記・紀神話の時代から、飛鳥、奈良、平安、鎌倉、南北朝、室町、戦国、織豊、江戸の各時代を経て現代に至るまで、政治・法律・思想・宗教・経済・産業・科学・社会・教育・文学・絵画・音楽・建築・工芸・芸能・スポーツなどあらゆる分野で活躍した人々を網羅した、日本最大規模の人名辞典です。");
	$(".title_50120").text("時代を動かすホット・パーソンに鋭く迫る、人物データ・コラム。政治、ビジネス、学術から、スポーツ、エンタテインメントまであらゆる分野で、今もっとも注目される人物をピックアップして、その基礎情報を速報します。");
	$(".title_40010").text("派生語・変化形・成句を含めて、最大の34万5,000語を収録しています。現代英語の語法・用法を詳説し、口語・俗語・成句や商品名・人名などの固有名詞、米国版に不足しているイギリス英語、カナダ・オーストラリア・ニュージーランドはもとよりアフリカ・中南米など第三世界の英語まで幅広く収録しています。");
	$(".title_40020").text("時事語、生活語、新語や俗語のほか、生命科学、金融、スポーツ用語など、学習のみならず、専門家の実務に活きる精選された11万7,000項目を収録しています。");
	$(".title_40030").text("日常生活に必要な基本語から各分野の専門語まで約9万項目が収録。また、「生きた英語」として標準的口語表現が増補されています。");
	$(".title_40050").text("故E.O.ライシャワー博士が監修し、名門ハーバード大学を中心とする一流の日本研究家による格調高く正確な英文で評価を得た、カラー版百科事典『Japan: An Illustrated Encyclopedia』（1993年　講談社刊）の本文を完全収録しています。");
	$(".title_40061").text("英英辞典の特徴はそのままに、日本人学習者向けに語句の意味・例文・説明などの日本語訳が加えられています。コウビルドの最大の特徴である、「フルセンテンスによる定義」で語句の意味を正確に理解し、語彙力だけでなく、各語句の実践的な用法や簡略で自然な英語表現も身につけられます。");
	$(".title_40070").text("ケンブリッジ・インターナショナル・コーパス （収録語彙数5億、うち英語を母国語としない学習者によって使われた語彙数1,500万語）に基づいて編纂された、学習者向けの英英辞典です。");
	$(".title_40100").text("研究社が発行する物理学と化学を中心に生命科学などの基本語、専門語など約4万語を収録した英和辞典です。訳語と発音に理化学な解説を加えたことで理化学辞典としました。");
	$(".title_40110").text("総収録語数15万。医学関連分野の用語を収録した医学英和辞典です。各分野の専門語のみならず、医学関連の一般語、略語、俗語なども収録しています。和英対照表を収録しているため、和英辞書としてもお使いいただけます。");
	$(".title_42150").text("新聞・雑誌に見られる新語のほか、時事用語、経済用語、情報用語などを充実させたドイツ語辞典です。");
	$(".title_42250").text("新語・専門語・略語を厳選し、新聞・雑誌を読むのに必要な成句を可能な限り採用したフランス語辞典です。");
	$(".title_42350").text("学習基本語から時事語・生活語・新語まで網羅したスペイン語辞典です。");
	$(".title_42450").text("美術・音楽・料理・サッカーなどの専門用語まで幅広く採録したイタリア語辞典です。");
	$(".title_42550").text("古ラテン語から近代の学術用語まで幅広く収録し、古典語学習者のみならず宗教音楽や動植物の学名などに関心のある方にも便利なラテン語辞典です。");
	$(".title_42151").text("新聞・雑誌に見られる新語のほか、時事用語、経済用語、情報用語などを充実させたドイツ語辞典です。");
	$(".title_42251").text("新語・専門語・略語を厳選し、新聞・雑誌を読むのに必要な成句を可能な限り採用したフランス語辞典です。");
	$(".title_42351").text("学習基本語から時事語・生活語・新語まで網羅したスペイン語辞典です。");
	$(".title_42451").text("美術・音楽・料理・サッカーなどの専門用語まで幅広く採録したイタリア語辞典です。");
	$(".title_42551").text("古ラテン語から近代の学術用語まで幅広く収録し、古典語学習者のみならず宗教音楽や動植物の学名などに関心のある方にも便利なラテン語辞典です。");
	$(".title_50010").text("政治、経済、国際情勢、社会、サイエンス、文化、スポーツなどから、約140の分野に大別された最新の用語解説を収録しています。なお、カラー図版約360点を用語解説とリンクさせ、ビジュアル面からも用語の理解をサポートします。");
	$(".title_50020").text("国際情勢や経済・政治・福祉・行政など日本の難局を明解に解説する専門知識・基礎語のみならず、文化・芸術・科学・技術・流行語・トリビアまで幅広くピックアップされています。");
	$(".title_50310").text("最新流行語・時事用語ウオッチャー亀井肇氏が、世の中に氾濫するIT関連用語、難しい経済・金融・経営専門用語を解説します。");
	$(".title_50410").text("日本国内の全上場企業について、特色や業績、財務内容、大株主、役員、株価動向などをコンパクトに網羅した企業データブックです。1936年（昭和11年）の創刊以来、「株式投資のバイブル」として広く親しまれているほか、取引先調査やマーケティング、就職情報としても活用されており、“会社辞典”の代名詞的な存在です。");
	$(".title_51010").text("紀元前27世紀の古代オリエントの神話文学から最新の現代文学まで、幅広くフォローした世界文学の足跡を集大成した大事典です。");
	$(".title_58060").text("物質レベルから地球規模の問題まで、化学のさまざまな分野を幅広く網羅した、実学重視の辞典です。基礎化学分野と応用化学分野に加え、広範な学際分野（原子力、放射線、エネルギー、生命科学、環境・公害など）の最新の情報も掲載しています。");
	$(".title_58110").text("世の中のさまざまな法則、定理を自然科学分野に的を絞って整理し、簡潔に解説を加えた辞典です。");
	$(".title_70010").text("毎日新聞発行の経済雑誌として、定評のある雑誌です。著名人による評論や経済の動きを躍動的に伝える充実したレポートが特徴です。");
	$(".title_70015").text("文藝春秋が発行する『日本の論点』の1997年版から最新版まで掲載されています。政治、経済、科学、医療、教育、スポーツなど様々なテーマの論点/論文を検索できます。");
	$(".title_70020").text("NNAが提供するアジアおよび欧州諸国の国際ニュースを検索できます。");
	$(".title_80110").text("記紀・萬葉などの古代文学、源氏物語などの中古文学、平家物語などの中世文学、そして近松・西鶴などの近世文学など珠玉の名作を公開し、難解な言い回しや語句の意味、作品の狙いや背景などを詳しく解説しています。");
	$(".title_60110").text("『日本大百科全書（ニッポニカ）』に掲載されている画像や動画、音声が検索できます。");
	
		$(this).parent().find(".title_explain").slideToggle(600);
	});

/* jknのウィンドウ */
	$(".open").live("click",function(){
	var date = new Date();
	var year = date.getFullYear();
	var month = date.getMonth() + 1;
	var day = date.getDate();
	if ( month < 10 ) {
　　	month = '0' + month;
	}
	if ( day < 10 ) {
　　	day = '0' + day;
	}
	var url = $(this).attr("data-jkn21-url");
	var title = $(this).attr("data-headword");
	
	
	
	/* クリックしたアイテムをcookieに格納  */
	var fav_item ="fav_item";
    var COOKIE_PATH = '/';
 
    var fav_array = [];
    if($.cookie(fav_item)){
        fav_array = $.cookie(fav_item).split("#-");
    }
 
	var fav_content = "<a href='"+url+"' target='_blank'>"+title+"</a>";

	if( $.inArray(fav_content, fav_array) == -1) {
        fav_array.push(fav_content);			
    }
    // 最大5件まで		
 	if(fav_array.length > 5) {
		fav_array.shift();
	}
    // パス(/)や有効期限(1日)を指定する
    var date = new Date();
    date.setTime(date.getTime() + ( 1000 * 60 * 60 * 24 * 1 ));
    $.cookie(fav_item, fav_array.join("#-"), { path: COOKIE_PATH, expires: date });
	fav_array.reverse();
	var fav_length = fav_array.length;
	var html = "";
	for (i = 0; i <  fav_length; i++) {
    	html += "<li>"+ fav_array[i] +"</li>";
	}
	$("#fav_list").html("<h5>閲覧したアイテム</h5><ul>"+ html +"</ul>");		
	
	var contentname = $(this).attr("data-contentname");
	$("#wrap_quote").remove();
	
	var win_with = window.screen.availWidth-500;
	var win_height = window.screen.availHeight-80;
	
	win1 = window.open(url,"","width=" + win_with +",height="+ win_height +",top=0,left=10,scrollbars=yes,resizable=yes");		 
	});



}
google.setOnLoadCallback(OnLoad);

</script>
</head>
<body id="top">

<div id="mcs3_container">
<div class="customScrollBox">
<div class="container">
<div class="content">

<div id="jkn21">
<header>
<h1>ジャパンナレッジ</h1>

<div class="explain"><p>
<?php
if($searchWord){
echo "ブラウザやWord、Excel上のテキストを選択するか、キーワードを入れて検索してください。";
}else{
echo "JapanKnowledgeとは、辞書、国際ニュース、書籍など知に関わる様々な情報を含む総合データベースです";
}
?>
</p></div>
</header>

<div id="jkn21_search_form">
<form autocomplete="off" action="desktop.php" method="get" class="iform" name="search">
<ul>
<li><input type="text" id="jkn21_text" name="searchWord" placeholder="キーワードを入力してください" value="" class="itext"><input type="hidden" name="currentPage" value="1"><input type="hidden" id="contentsSelect" name="contentsSelect" value="all"> <button type="submit" id="jkn21_search_button" class="jkn21_button" role="button">検索</button></li>
</ul>
<div class="searchForm-act"><input name="flowMode" type="radio" class="radio" id="flowMode1" value="1" checked="checked"><label for="flowMode1" class="radiolabel <?php if(($flowMode=='1')or(empty($_GET['flowMode']))){echo 'RadioSelected';} ?>" id="radio_dictionary">辞事典系</label><input name="flowMode" type="radio" class="radio" id="flowMode2" value="2"><label for="flowMode2" class="radiolabel <?php if($flowMode=='2')echo 'RadioSelected'; ?>" id="radio_article">記事叢書</label><input name="flowMode" type="radio" class="radio" id="flowMode3" value="3"><label for="flowMode3" class="radiolabel <?php if($flowMode=='3')echo 'RadioSelected'; ?>" id="radio_all">全文</label> <span id="jkn21_search_detail" class="open_detail">詳細</span></div>
</form>
</div>

<?php
if($searchWord){
function get_xml($searchWord,$contentsSelect,$currentPage,$flowMode){
	//ニッポニカURLを除外
	if( $flowMode==2 && $contentsSelect == "all"){
		$contentsSelect = "60110,70010,70015,70020,80010,80110";
	} elseif ($flowMode==3 && $contentsSelect == "all") {
		$contentsSelect ="10010,10011,20020,20010,20030,20040,30010,30020,30030,30100,50110,50120,40010,40020,40030,40050,40061,40070,40100,40110,42150,42250,42350,42450,42550,42151,42251,42351,42451,42551,50010,50020,50310,50410,51010,58060,58110,60110,70010,70015,70020,80010,80110";
	}
	
if (defined("PROXY_URL")) {
	$proxy_opts = array(
	'http' => array(
	'proxy' => 'tcp://' . PROXY_URL,
	'request_fulluri' => true,
	),
	);
	$proxy_context=stream_context_create($proxy_opts);
} else {
	$proxy_context=null;
}
$raw = file_get_contents('http://www.jkn21.com/jkws/searchlist?' . CONTRACT_KEY . '&corpType=www&searchWord='.urlencode($searchWord).'&contentsSelect='.$contentsSelect.'&flowMode='.$flowMode.'&currentPage='.$currentPage,false,$proxy_context);
$xml = simplexml_load_string($raw);
$item_count = $xml->result->count;
echo "<div id='jkn21_result_list'>";

$item_count = $xml->result->count;
$errorcode = $xml->result->errorcode;
$hits = $xml->result->searchlist;

switch ($errorcode) {
    case "JKWS01ERR002":
        echo "<p class='ui-state-error'>検索条件が正しくありません。</p>";
        break;
    case "JKWS01ERR003":
        echo "<p class='ui-state-error'>検索結果件数が上限を超えています。<br />複数のキーワードで再度検索してください。</p>";
        break;
    case "JKWS01ERR009":
        echo "<p class='ui-state-error'>JapanKnowledgeでエラーが発生しております。</p>";
        break;
}

if(!$errorcode && $item_count==0){
	echo "<p class='ui-state-error'>該当するデータがありませんでした。</p>";
	complete($searchWord,$contentsSelect,$flowMode,"show");	
}

if(!$errorcode && $item_count>0){

switch ($flowMode) {
	case "1":
		echo "<div id='result_nav'><span class='dictionary_result_num'>検索結果 ： ".$item_count."件</span></div>";
	break;
    case "2":
		echo "<div id='result_nav'><span class='article_result_num'>検索結果 ： ".$item_count."件</span></div>";
	break;
    case "3":
		echo "<div id='result_nav'><span class='all_result_num'>検索結果 ： ".$item_count."件</span></div>";
	break;
}

}

//complete($searchWord,$contentsSelect,$flowMode,"hide");	


echo "<p id='copyright'>Powered by BUKKYO UNIVERSITY LIBRARY Assist by CMS</p>";



foreach ($hits as $hit) {
	$headword = $hit->headword;
	$contentsname = $hit->contentsname;
	$snippet = $hit->snippet;
	$titleid = $hit->titleid;
	$localid = $hit->localid;
	$ip_address = getenv('REMOTE_ADDR');
	$host_name = gethostbyaddr($ip_address);
	$url = "http://www.jkn21.com/jkws/bodydisplay?" . CONTRACT_KEY . "&corpType=www&titleid={$titleid}&localid={$localid}";

	if((defined("SNIPPET_DISP_HOST") and strlen(SNIPPET_DISP_HOST) and (preg_match("/" . SNIPPET_DISP_HOST . "/", $host_name))) or (defined("SNIPPET_DISP_ADDR") and strlen(SNIPPET_DISP_ADDR) and (preg_match("/" . SNIPPET_DISP_ADDR. "/", $ip_address)))){
	if($titleid !=="89010"){
	echo "<div class='jkn21_item open' data-jkn21-url='".$url."' data-headword='".htmlspecialchars($hit->headword, ENT_QUOTES)."' data-contentname='".$contentsname."'><h3><span>".$headword."</span> （".$contentsname."）</h3>";
	echo "<p>".$snippet."</p>";
	echo "</div>";
	}else{
		echo "";	
	}
	}else{
	echo "<div class='jkn21_item open' data-jkn21-url='".$url."' data-headword='".htmlspecialchars($hit->headword, ENT_QUOTES)."' data-contentname='".$contentsname."'><h3><span>".$headword."</span> （".$contentsname."）</h3>";
	echo "</div>";		
	}
}

echo "</div>";

$nextPage = $currentPage + 1;
$prevPage = $currentPage -1;

if($item_count >10){
	echo "<div id='jkn21_pagenation' class='clearfix'>";
	if($currentPage >1){
	echo "<span id='prev_page'><a href='desktop.php?searchWord=".$searchWord."&currentPage=".$prevPage."&contentsSelect=".$contentsSelect."&flowMode=".$flowMode."'>前へ</a></span>";
	}
	if($flowMode == 1 && $item_count/10 > $currentPage ){
	echo "<span id='next_page'><a href='desktop.php?searchWord=".$searchWord."&currentPage=".$nextPage."&contentsSelect=".$contentsSelect."&flowMode=".$flowMode."'>次へ</a></span>";
	}
	if($flowMode == 2 && $item_count/10 > $currentPage ){
	echo "<span id='next_page'><a href='desktop.php?searchWord=".$searchWord."&currentPage=".$nextPage."&contentsSelect=".$contentsSelect."&flowMode=".$flowMode."'>次へ</a></span>";
	}
	if($flowMode == 3 && $item_count/20 > $currentPage ){
	echo "<span id='next_page'><a href='desktop.php?searchWord=".$searchWord."&currentPage=".$nextPage."&contentsSelect=".$contentsSelect."&flowMode=".$flowMode."'>次へ</a></span>";
	}
	echo "</div>";
}


}

get_xml($searchWord,$contentsSelect,$currentPage,$flowMode);

}else{
	
	echo "<div id='info_contents'><h2>いつでもジャパンナレッジ</h2><div id='wrap_info'><div class='active' id='info01'><img src='" . IMG_DIR . "about.png' width='233px' height='155px' /><p>ブラウザやWord、Excel上のテキストをマウスで選択（反転）させて、検索ボタンをクリックするだけで、いつでもジャパンナレッジを検索することができます。</p><p>また、検索窓にキーワードを入力して検索することも可能です。</p></div></div></div>";


}

?>
</div>


</div>
</div>
<div class="dragger_container">
<div class="dragger"></div>
</div>
</div>
</div>

<div id='history_content'><h4>過去5件の履歴</h4><p><span id='delete_history'>》　履歴を消去</span></p><p class='close_history'>閉じる</p><div id='kywd_list'></div><div id='history_list'></div><div id='fav_list'></div></div>

<footer><span id='link_home'><a href='desktop.php'>link to Home</a></span><span id='print'><a href='#'>このページを印刷する</a></span><div id='yourkywd'></div></footer>

<?php

echo ("<script type=\"text/javascript\" src=\"" . JS_DIR . "jquery.easing.1.3.js\"></script>");
echo ("<script src=\"" . JS_DIR . "jquery.mousewheel.min.js\"></script>");
echo ("<script src=\"" . JS_DIR . "jquery.mCustomScrollbar.js\"></script>");
echo ("<script type=\"text/javascript\" src=\"" . JS_DIR . "jquery.cookie.js\"></script>");
?>
<script>

$(document).ready(function(){

/* プリンタ  */						   
$('#print').click(function(){
	window.print();
	return false;
});

/* フェイドインで表示 */
$("#jkn21_result_list").fadeIn(800);
$(".explain").fadeIn(800);


/* hoverでアニメーション */
$("nav").hover(function(){
	$(this).animate({
    backgroundColor: "black"
  }, 1000 );	
},function(){
	$(this).animate({
    backgroundColor: "#666"
  }, 1000 );
});

$("footer").hover(function(){
	$(this).animate({
    backgroundColor: "#000"
  }, 1000 );	
},function(){
	$(this).animate({
    backgroundColor: "#333"
  }, 1000 );
});

$(".jkn21_item").hover(function(){
	$(this).animate({
   backgroundColor: "#EEE"
  }, 500 );	
},function(){
	$(this).animate({
   backgroundColor: "#FDFDFD"
  }, 500 );
});

$(".radiolabel").hover(function(){
	$(this).animate({
   backgroundColor: "#555"
  }, 500 );	
},function(){
	$(this).animate({
   backgroundColor: "#000"
  }, 500 );
});

$(".radio").change(function(){
	if($(this).is(":checked")){
	$(".RadioSelected:not(:checked)").removeClass("RadioSelected");
	$(this).next("label").addClass("RadioSelected");
}
});

function disable_links_outline() {
　　var blur = function () { this.blur() };
　　for (var i = 0; i < document.links.length; i++)
　　　　document.links[i].onfocus = blur;
}

disable_links_outline();



mCustomScrollbars();
						   

function mCustomScrollbars(){
	$("#mcs3_container").mCustomScrollbar("vertical",window.screen.availHeight,"easeOutCirc",1.05,"auto","yes","no",10); 
}

/* function to fix the -10000 pixel limit of jquery.animate */
$.fx.prototype.cur = function(){
    if ( this.elem[this.prop] != null && (!this.elem.style || this.elem.style[this.prop] == null) ) {
      return this.elem[ this.prop ];
    }
    var r = parseFloat( jQuery.css( this.elem, this.prop ) );
    return typeof r == 'undefined' ? 0 : r;
}
						   

//パラメータ取得
function getUrlVars(){
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for(var i = 0; i <hashes.length; i++)
    	{
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    	}
    return vars;
};

    var yourkywd ="yourkywd";
    var COOKIE_PATH = '/';
 
    var kywd_array = [];
    if($.cookie(yourkywd)){
        kywd_array = $.cookie(yourkywd).split("-");
    }
 
    // 現在開いているページのキーワード
	var kywd = getUrlVars()["searchWord"];

	if(getUrlVars()["currentPage"]=="1"){
    	if( $.inArray(kywd, kywd_array) == -1) {
        	// cookieから取得した配列内に存在しない場合は追加
        	kywd_array.push(kywd);			
    	}
        	// 最大5件まで		
 			if(kywd_array.length > 5) {
				kywd_array.shift();
			}
	}
 
 
    // パス(/)や有効期限(1日)を指定する
    var date = new Date();
    date.setTime(date.getTime() + ( 1000 * 60 * 60 * 24 * 1 ));
    $.cookie(yourkywd, kywd_array.join("-"), { path: COOKIE_PATH, expires: date });
	
	var kywd_length = kywd_array.length;
	if(kywd_length > 1){
		var kywd = decodeURI(kywd_array[kywd_length-2]);
		
		if (kywd.length > 8) {
		// kywdが8文字より大きい場合
		kywd = kywd.substr(0, 8) + '…';
		}
		
	$("#yourkywd").html("前回の検索 ： <a href='desktop.php?searchWord="+ kywd_array[kywd_length-2] +"&currentPage=1&contentsSelect=all&flowMode=1'>"+ kywd +"</a> <span id='history' class='open_history'>履歴</span>");
	}else{
	$("#yourkywd").html("前回の検索 ：  <span id='history' class='open_history'>履歴</span>");		
}

$(".open_history").live("click",function(){
	$(this).removeClass("open_history");
 	$("#jkn21_pagenation").hide();
 	$("#jkn21").css({"opacity":"0.3"});	

	kywd_array.reverse();

var html = "";
for (i = 0; i < kywd_array.length; i++) {
    html += "<li><a href='desktop.php?searchWord="+ kywd_array[i] +"&currentPage=1&contentsSelect=all&flowMode=1'>"+decodeURI(kywd_array[i])+"</a></li>";
}

    var fav_item ="fav_item";
    if($.cookie(fav_item)){
        var fav_array = $.cookie(fav_item).split("#-");
		fav_array.reverse();
		var fav_length = fav_array.length;
		if(fav_length > 0){
		var fav_html = "";
		for (i = 0; i <  fav_length; i++) {
    		fav_html += "<li>"+ fav_array[i] +"</li>";
		}
		$("#fav_list").html("<h5>閲覧したアイテム</h5><ul>"+ fav_html +"</ul>");
		$("#fav_list").fadeIn();
		}
		
    }

	$("#kywd_list").html("<h5>キーワード</h5><ul>"+html+"</ul>");
	$("#history_content").fadeIn(400);	
});


$(".close_history").live("click",function(){
 	$("#jkn21").css({"opacity":"1"});	
	$("#history_content").fadeOut(800);
	$("#jkn21_pagenation").fadeIn(200);
	$("#history").addClass("open_history");
});




	$("#delete_history").click(function(){
	$("#kywd_list").remove();
	$("#history_list").remove();
	$("#fav_list").remove();
	$(this).text("履歴を消去しました。");
	$.cookie("yourkywd","",{path:"/",expires:-1});	
	$.cookie("fav_item","",{path:"/",expires:-1});	
	
	
	});	

})
</script>

</body>
</html>
