<?php
/*
 * config.php
 * JKDesktopSearch Proxy Server CGI config file
 *
 * Powerd by BUKKYO UNIVERSITY LIBRARY Assit by CMS
 *
 */

// JapanKnowledge�Ȥη��󥭡�
define("CONTRACT_KEY", "XXXXXXXXXX");

// javascript��Ǽ�ե����̾(PHP��Ʊ���ʤ��ά��)
define("JS_DIR", "");

// CSS��Ǽ�ե����̾(PHP��Ʊ���ʤ��ά��)
define("CSS_DIR", "");

// ���᡼����Ǽ�ե����̾��������ѹ��������ϳ�CSS�ե��������url���ѹ�����ɬ�פ�����ޤ�
define("IMG_DIR", CSS_DIR . "/images/");

// �ץ���URL
//define("PROXY_URL", "proxy.exsample.ac.jp:8080");

// ������̤ؤΥ��˥ڥå�ɽ������Ĥ���REMOTE_ADDR(����ɽ��)
define("SNIPPET_DISP_ADDR", "192\\.168\\.1\\.");

// ������̤ؤΥ��˥ڥå�ɽ������Ĥ���REMOTE_HOST(����ɽ��)
define("SNIPPET_DISP_HOST", "exsample\\.ac\\.jp");
?>
