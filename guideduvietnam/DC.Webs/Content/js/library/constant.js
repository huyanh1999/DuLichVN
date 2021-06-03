// Regular Expression
var SITENAME_EXPRESSION = /^[a-z0-9-]{2,32}$/;
var DOMAINNAME_EXPRESSION = /^[a-zA-Z0-9][a-zA-Z0-9-]{1,61}[a-zA-Z0-9](?:\.[a-zA-Z]{2,})+$/;
var SLUGNAME_EXPRESSION = /^[a-zA-Z0-9-?&#=]+$/;
var URLNAME_EXPRESSION = /^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)?$/;
var EMAILADDRESS_EXPRESSION = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
var HEXNAME_EXPRESSION = /^#?([a-f0-9]{6}|[a-f0-9]{3})$/;
var USERNAME_EXPRESSION = /^[a-zA-Z0-9]+$/;
var PASSWORD_EXPRESSION = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,20}/;

var PHONENAME_EXPRESSION = /^[+]*[(]{0,1}[0-9]{1,3}[)]{0,1}[-\s\./0-9]*$/;
var DATETIMENAME_EXPRESSION = /^(0?[1-9]|[12][0-9]|3[01])\/(0?[1-9]|1[012])\/(19\d\d|(20)?[0-7][0-8])$/;
var NUMBER_EXPRESSION = /^\d+$/;