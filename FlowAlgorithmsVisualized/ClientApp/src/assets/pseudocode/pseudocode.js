(function (e) {
  if (typeof exports === "object" && typeof module !== "undefined") {
    module.exports = e()
  } else if (typeof define === "function" && define.amd) {
    define([], e)
  } else {
    var t;
    if (typeof window !== "undefined") {
      t = window
    } else if (typeof global !== "undefined") {
      t = global
    } else if (typeof self !== "undefined") {
      t = self
    } else {
      t = this
    }
    t.pseudocode = e()
  }
}
)(function () {
  var e, t, n;
  return function () {
    function p(o, s, a) {
      function l(n, e) {
        if (!s[n]) {
          if (!o[n]) {
            var t = "function" == typeof require && require;
            if (!e && t)
              return t(n, !0);
            if (h)
              return h(n, !0);
            var i = new Error("Cannot find module '" + n + "'");
            throw i.code = "MODULE_NOT_FOUND",
            i
          }
          var r = s[n] = {
            exports: {}
          };
          o[n][0].call(r.exports, function (e) {
            var t = o[n][1][e];
            return l(t || e)
          }, r, r.exports, p, o, s, a)
        }
        return s[n].exports
      }
      for (var h = "function" == typeof require && require, e = 0; e < a.length; e++)
        l(a[e]);
      return l
    }
    return p
  }()({
    1: [function (e, t, n) {
      var i = e("./src/ParseError");
      var r = e("./src/Lexer");
      var o = e("./src/Parser");
      var s = e("./src/Renderer");
      function a(e, t) {
        var n = new r(e);
        var i = new o(n);
        return new s(i, t)
      }
      function l(t) {
        try {
          MathJax.typeset([t])
        } catch (e) {
          MathJax.Hub.Queue(["Typeset", MathJax.Hub, t])
        }
      }
      t.exports = {
        ParseError: i,
        render: function (e, t, n) {
          if (e === null || e === undefined)
            throw "input cannot be empty";
          var i = a(e, n);
          var r = i.toDOM();
          if (t)
            t.appendChild(r);
          if (i.backend.name === "mathjax") {
            l(r)
          }
          return r
        },
        renderToString: function (e, t) {
          if (e === null || e === undefined)
            throw "input cannot be empty";
          var n = a(e, t);
          if (n.backend.name === "mathjax") {
            console.warn("Using MathJax backend -- math may not be rendered.")
          }
          return n.toMarkup()
        },
        renderElement: function (e, t) {
          if (!(e instanceof Element))
            throw "a DOM element is required";
          e.style.display = "none";
          var n = a(e.textContent, t);
          var i = n.toDOM();
          e.replaceWith(i);
          if (n.backend) {
            if (n.backend.name === "mathjax") {
              l(i)
            }
          }
        },
        renderClass: function (e, t) {
          [].forEach.call(document.getElementsByClassName(e), function (e) {
            this.renderElement(e, t)
          })
        }
      }
    }
      , {
      "./src/Lexer": 2,
      "./src/ParseError": 3,
      "./src/Parser": 4,
      "./src/Renderer": 5
    }],
    2: [function (e, t, n) {
      var i = e("./utils");
      var u = e("./ParseError");
      var r = function (e) {
        this._input = e;
        this._remain = e;
        this._pos = 0;
        this._nextAtom = this._currentAtom = null;
        this._next()
      };
      r.prototype.accept = function (e, t) {
        if (this._nextAtom.type === e && this._matchText(t)) {
          this._next();
          return this._currentAtom.text
        }
        return null
      }
        ;
      r.prototype.expect = function (e, t) {
        var n = this._nextAtom;
        if (n.type !== e)
          throw new u("Expect an atom of " + e + " but received " + n.type, this._pos, this._input);
        if (!this._matchText(t))
          throw new u("Expect `" + t + "` but received `" + n.text + "`", this._pos, this._input);
        this._next();
        return this._currentAtom.text
      }
        ;
      r.prototype.get = function () {
        return this._currentAtom
      }
        ;
      var o = {
        exec: function (e) {
          var t = [{
            start: "$",
            end: "$"
          }, {
            start: "\\(",
            end: "\\)"
          }];
          var n = e.length;
          for (var i = 0; i < t.length; i++) {
            var r = t[i].start;
            if (e.indexOf(r) !== 0)
              continue;
            var o = t[i].end;
            var s = r.length;
            var a = e.slice(s);
            while (s < n) {
              var l = a.indexOf(o);
              if (l < 0)
                throw new u("Math environment is not closed", this._pos, this._input);
              if (l > 0 && a[l - 1] === "\\") {
                var h = l + o.length;
                a = a.slice(h);
                s += h;
                continue
              }
              var p = [e.slice(0, s + l + o.length), e.slice(r.length, s + l)];
              return p
            }
          }
          return null
        }
      };
      var p = {
        special: /^(\\\\|\\{|\\}|\\\$|\\&|\\#|\\%|\\_)/,
        math: o,
        func: /^\\([a-zA-Z]+)/,
        open: /^\{/,
        close: /^\}/,
        quote: /^(`|``|'|'')/,
        ordinary: /^[^\\{}$&#%_\s]+/
      };
      var c = /^%.*/;
      var f = /^\s+/;
      r.prototype._skip = function (e) {
        this._pos += e;
        this._remain = this._remain.slice(e)
      }
        ;
      r.prototype._next = function () {
        var e = false;
        while (1) {
          var t = f.exec(this._remain);
          if (t) {
            e = true;
            var n = t[0].length;
            this._skip(n)
          }
          var i = c.exec(this._remain);
          if (!i)
            break;
          var r = i[0].length;
          this._skip(r)
        }
        this._currentAtom = this._nextAtom;
        if (this._remain === "") {
          this._nextAtom = {
            type: "EOF",
            text: null,
            whitespace: false
          };
          return false
        }
        for (var o in p) {
          var s = p[o];
          var a = s.exec(this._remain);
          if (!a)
            continue;
          var l = a[0];
          var h = a[1] ? a[1] : l;
          this._nextAtom = {
            type: o,
            text: h,
            whitespace: e
          };
          this._pos += l.length;
          this._remain = this._remain.slice(a[0].length);
          return true
        }
        throw new u("Unrecoganizable atom", this._pos, this._input)
      }
        ;
      r.prototype._matchText = function (e) {
        if (e === null || e === undefined)
          return true;
        if (i.isString(e))
          return e.toLowerCase() === this._nextAtom.text.toLowerCase();
        else {
          e = e.map(function (e) {
            return e.toLowerCase()
          });
          return e.indexOf(this._nextAtom.text.toLowerCase()) >= 0
        }
      }
        ;
      t.exports = r
    }
      , {
      "./ParseError": 3,
      "./utils": 6
    }],
    3: [function (e, t, n) {
      function i(e, t, n) {
        var i = "Error: " + e;
        if (t !== undefined && n !== undefined) {
          i += " at position " + t + ": `";
          n = n.slice(0, t) + "\u21b1" + n.slice(t);
          var r = Math.max(0, t - 15);
          var o = t + 15;
          i += n.slice(r, o) + "`"
        }
        this.message = i
      }
      i.prototype = Object.create(Error.prototype);
      i.prototype.constructor = i;
      t.exports = i
    }
      , {}],
    4: [function (e, t, n) {
      var s = e("./utils");
      var r = e("./ParseError");
      var a = function (e, t) {
        this.type = e;
        this.value = t;
        this.children = []
      };
      a.prototype.toString = function (e) {
        if (!e)
          e = 0;
        var t = "";
        for (var n = 0; n < e; n++)
          t += "  ";
        var i = t + "<" + this.type + ">";
        if (this.value)
          i += " (" + s.toString(this.value) + ")";
        i += "\n";
        if (this.children) {
          for (var r = 0; r < this.children.length; r++) {
            var o = this.children[r];
            i += o.toString(e + 1)
          }
        }
        return i
      }
        ;
      a.prototype.addChild = function (e) {
        if (!e)
          throw "argument cannot be null";
        this.children.push(e)
      }
        ;
      var o = function (e, t, n) {
        this.type = e;
        this.value = t;
        this.children = null;
        this.whitespace = !!n
      };
      o.prototype = a.prototype;
      var i = function (e) {
        this._lexer = e
      };
      i.prototype.parse = function () {
        var e = new a("root");
        while (true) {
          var t = this._acceptEnvironment();
          if (t === null)
            break;
          var n;
          if (t === "algorithm")
            n = this._parseAlgorithmInner();
          else if (t === "algorithmic")
            n = this._parseAlgorithmicInner();
          else
            throw new r("Unexpected environment " + t);
          this._closeEnvironment(t);
          e.addChild(n)
        }
        this._lexer.expect("EOF");
        return e
      }
        ;
      i.prototype._acceptEnvironment = function () {
        var e = this._lexer;
        if (!e.accept("func", "begin"))
          return null;
        e.expect("open");
        var t = e.expect("ordinary");
        e.expect("close");
        return t
      }
        ;
      i.prototype._closeEnvironment = function (e) {
        var t = this._lexer;
        t.expect("func", "end");
        t.expect("open");
        t.expect("ordinary", e);
        t.expect("close")
      }
        ;
      i.prototype._parseAlgorithmInner = function () {
        var e = new a("algorithm");
        while (true) {
          var t = this._acceptEnvironment();
          if (t !== null) {
            if (t !== "algorithmic")
              throw new r("Unexpected environment " + t);
            var n = this._parseAlgorithmicInner();
            this._closeEnvironment();
            e.addChild(n);
            continue
          }
          var i = this._parseCaption();
          if (i) {
            e.addChild(i);
            continue
          }
          break
        }
        return e
      }
        ;
      i.prototype._parseAlgorithmicInner = function () {
        var e = new a("algorithmic");
        var t;
        while (true) {
          t = this._parseStatement(l);
          if (t) {
            e.addChild(t);
            continue
          }
          t = this._parseBlock();
          if (t.children.length > 0) {
            e.addChild(t);
            continue
          }
          break
        }
        return e
      }
        ;
      i.prototype._parseCaption = function () {
        var e = this._lexer;
        if (!e.accept("func", "caption"))
          return null;
        var t = new a("caption");
        e.expect("open");
        t.addChild(this._parseCloseText());
        e.expect("close");
        return t
      }
        ;
      i.prototype._parseBlock = function () {
        var e = new a("block");
        while (true) {
          var t = this._parseControl();
          if (t) {
            e.addChild(t);
            continue
          }
          var n = this._parseFunction();
          if (n) {
            e.addChild(n);
            continue
          }
          var i = this._parseStatement(h);
          if (i) {
            e.addChild(i);
            continue
          }
          var r = this._parseCommand(p);
          if (r) {
            e.addChild(r);
            continue
          }
          var o = this._parseComment();
          if (o) {
            e.addChild(o);
            continue
          }
          break
        }
        return e
      }
        ;
      i.prototype._parseControl = function () {
        var e;
        if (e = this._parseIf())
          return e;
        if (e = this._parseLoop())
          return e;
        if (e = this._parseRepeat())
          return e;
        if (e = this._parseUpon())
          return e
      }
        ;
      i.prototype._parseFunction = function () {
        var e = this._lexer;
        if (!e.accept("func", ["function", "procedure"]))
          return null;
        var t = this._lexer.get().text;
        e.expect("open");
        var n = e.expect("ordinary");
        e.expect("close");
        e.expect("open");
        var i = this._parseCloseText();
        e.expect("close");
        var r = this._parseBlock();
        e.expect("func", "end" + t);
        var o = new a("function", {
          type: t,
          name: n
        });
        o.addChild(i);
        o.addChild(r);
        return o
      }
        ;
      i.prototype._parseIf = function () {
        if (!this._lexer.accept("func", "if"))
          return null;
        var e = new a("if");
        this._lexer.expect("open");
        e.addChild(this._parseCond());
        this._lexer.expect("close");
        e.addChild(this._parseBlock());
        var t = 0;
        while (this._lexer.accept("func", ["elif", "elsif", "elseif"])) {
          this._lexer.expect("open");
          e.addChild(this._parseCond());
          this._lexer.expect("close");
          e.addChild(this._parseBlock());
          t++
        }
        var n = false;
        if (this._lexer.accept("func", "else")) {
          n = true;
          e.addChild(this._parseBlock())
        }
        this._lexer.expect("func", "endif");
        e.value = {
          numElif: t,
          hasElse: n
        };
        return e
      }
        ;
      i.prototype._parseLoop = function () {
        if (!this._lexer.accept("func", ["FOR", "FORALL", "WHILE"]))
          return null;
        var e = this._lexer.get().text.toLowerCase();
        var t = new a("loop", e);
        this._lexer.expect("open");
        t.addChild(this._parseCond());
        this._lexer.expect("close");
        t.addChild(this._parseBlock());
        var n = e !== "forall" ? "end" + e : "endfor";
        this._lexer.expect("func", n);
        return t
      }
        ;
      i.prototype._parseRepeat = function () {
        if (!this._lexer.accept("func", ["REPEAT"]))
          return null;
        var e = this._lexer.get().text.toLowerCase();
        var t = new a("repeat", e);
        t.addChild(this._parseBlock());
        this._lexer.expect("func", "until");
        this._lexer.expect("open");
        t.addChild(this._parseCond());
        this._lexer.expect("close");
        return t
      }
        ;
      i.prototype._parseUpon = function () {
        if (!this._lexer.accept("func", "upon"))
          return null;
        var e = new a("upon");
        this._lexer.expect("open");
        e.addChild(this._parseCond());
        this._lexer.expect("close");
        e.addChild(this._parseBlock());
        this._lexer.expect("func", "endupon");
        return e
      }
        ;
      var l = ["ensure", "require", "input", "output"];
      var h = ["state", "print", "return"];
      i.prototype._parseStatement = function (e) {
        if (!this._lexer.accept("func", e))
          return null;
        var t = this._lexer.get().text.toLowerCase();
        var n = new a("statement", t);
        n.addChild(this._parseOpenText());
        return n
      }
        ;
      var p = ["break", "continue"];
      i.prototype._parseCommand = function (e) {
        if (!this._lexer.accept("func", e))
          return null;
        var t = this._lexer.get().text.toLowerCase();
        var n = new a("command", t);
        return n
      }
        ;
      i.prototype._parseComment = function () {
        if (!this._lexer.accept("func", "comment"))
          return null;
        var e = new a("comment");
        this._lexer.expect("open");
        e.addChild(this._parseCloseText());
        this._lexer.expect("close");
        return e
      }
        ;
      i.prototype._parseCall = function () {
        var e = this._lexer;
        if (!e.accept("func", "call"))
          return null;
        var t = e.get().whitespace;
        e.expect("open");
        var n = e.expect("ordinary");
        e.expect("close");
        var i = new a("call");
        i.whitespace = t;
        i.value = n;
        e.expect("open");
        var r = this._parseCloseText();
        i.addChild(r);
        e.expect("close");
        return i
      }
        ;
      i.prototype._parseCond = i.prototype._parseCloseText = function () {
        return this._parseText("close")
      }
        ;
      i.prototype._parseOpenText = function () {
        return this._parseText("open")
      }
        ;
      i.prototype._parseText = function (e) {
        var t = new a(e + "-text");
        var n = false;
        var i;
        while (true) {
          i = this._parseAtom() || this._parseCall();
          if (i) {
            if (n)
              i.whitespace |= n;
            t.addChild(i);
            continue
          }
          if (this._lexer.accept("open")) {
            i = this._parseCloseText();
            n = this._lexer.get().whitespace;
            i.whitespace = n;
            t.addChild(i);
            this._lexer.expect("close");
            n = this._lexer.get().whitespace;
            continue
          }
          break
        }
        return t
      }
        ;
      var u = {
        ordinary: {
          tokenType: "ordinary"
        },
        math: {
          tokenType: "math"
        },
        special: {
          tokenType: "special"
        },
        "cond-symbol": {
          tokenType: "func",
          tokenValues: ["and", "or", "not", "true", "false", "to", "downto"]
        },
        "quote-symbol": {
          tokenType: "quote"
        },
        "sizing-dclr": {
          tokenType: "func",
          tokenValues: ["tiny", "scriptsize", "footnotesize", "small", "normalsize", "large", "Large", "LARGE", "huge", "Huge"]
        },
        "font-dclr": {
          tokenType: "func",
          tokenValues: ["normalfont", "rmfamily", "sffamily", "ttfamily", "upshape", "itshape", "slshape", "scshape", "bfseries", "mdseries", "lfseries"]
        },
        "font-cmd": {
          tokenType: "func",
          tokenValues: ["textnormal", "textrm", "textsf", "texttt", "textup", "textit", "textsl", "textsc", "uppercase", "lowercase", "textbf", "textmd", "textlf"]
        },
        "text-symbol": {
          tokenType: "func",
          tokenValues: ["textbackslash"]
        }
      };
      i.prototype._parseAtom = function () {
        for (var e in u) {
          var t = u[e];
          var n = this._lexer.accept(t.tokenType, t.tokenValues);
          if (n === null)
            continue;
          var i = this._lexer.get().whitespace;
          if (e !== "ordinary" && e !== "math")
            n = n.toLowerCase();
          return new o(e, n, i)
        }
        return null
      }
        ;
      t.exports = i
    }
      , {
      "./ParseError": 3,
      "./utils": 6
    }],
    5: [function (n, e, t) {
      var a = n("./utils");
      function A(e) {
        this._css = {};
        this._fontSize = this._outerFontSize = e !== undefined ? e : 1
      }
      A.prototype.outerFontSize = function (e) {
        if (e !== undefined)
          this._outerFontSize = e;
        return this._outerFontSize
      }
        ;
      A.prototype.fontSize = function () {
        return this._fontSize
      }
        ;
      A.prototype._fontCommandTable = {
        normalfont: {
          "font-family": "KaTeX_Main"
        },
        rmfamily: {
          "font-family": "KaTeX_Main"
        },
        sffamily: {
          "font-family": "KaTeX_SansSerif"
        },
        ttfamily: {
          "font-family": "KaTeX_Typewriter"
        },
        bfseries: {
          "font-weight": "bold"
        },
        mdseries: {
          "font-weight": "medium"
        },
        lfseries: {
          "font-weight": "lighter"
        },
        upshape: {
          "font-style": "normal",
          "font-variant": "normal"
        },
        itshape: {
          "font-style": "italic",
          "font-variant": "normal"
        },
        scshape: {
          "font-style": "normal",
          "font-variant": "small-caps"
        },
        slshape: {
          "font-style": "oblique",
          "font-variant": "normal"
        },
        textnormal: {
          "font-family": "KaTeX_Main"
        },
        textrm: {
          "font-family": "KaTeX_Main"
        },
        textsf: {
          "font-family": "KaTeX_SansSerif"
        },
        texttt: {
          "font-family": "KaTeX_Typewriter"
        },
        textbf: {
          "font-weight": "bold"
        },
        textmd: {
          "font-weight": "medium"
        },
        textlf: {
          "font-weight": "lighter"
        },
        textup: {
          "font-style": "normal",
          "font-variant": "normal"
        },
        textit: {
          "font-style": "italic",
          "font-variant": "normal"
        },
        textsc: {
          "font-style": "normal",
          "font-variant": "small-caps"
        },
        textsl: {
          "font-style": "oblique",
          "font-variant": "normal"
        },
        uppercase: {
          "text-transform": "uppercase"
        },
        lowercase: {
          "text-transform": "lowercase"
        }
      };
      A.prototype._sizingScalesTable = {
        tiny: .68,
        scriptsize: .8,
        footnotesize: .85,
        small: .92,
        normalsize: 1,
        large: 1.17,
        Large: 1.41,
        LARGE: 1.58,
        huge: 1.9,
        Huge: 2.28
      };
      A.prototype.updateByCommand = function (e) {
        var t = this._fontCommandTable[e];
        if (t !== undefined) {
          for (var n in t)
            this._css[n] = t[n];
          return
        }
        var i = this._sizingScalesTable[e];
        if (i !== undefined) {
          this._outerFontSize = this._fontSize;
          this._fontSize = i;
          return
        }
        throw new ParserError("unrecogniazed text-style command")
      }
        ;
      A.prototype.toCSS = function () {
        var e = "";
        for (var t in this._css) {
          var n = this._css[t];
          if (n === undefined)
            continue;
          e += t + ":" + n + ";"
        }
        if (this._fontSize !== this._outerFontSize) {
          e += "font-size:" + this._fontSize / this._outerFontSize + "em;"
        }
        return e
      }
        ;
      function B(e, t) {
        this._nodes = e;
        this._textStyle = t
      }
      B.prototype._renderCloseText = function (e, t) {
        var n = new A(this._textStyle.fontSize());
        var i = new B(e.children, n);
        if (e.whitespace)
          this._html.putText(" ");
        this._html.putHTML(i.renderToHTML(t))
      }
        ;
      B.prototype.renderToHTML = function (e) {
        this._html = new _;
        var t;
        while ((t = this._nodes.shift()) !== undefined) {
          var n = t.type;
          var i = t.value;
          if (t.whitespace)
            this._html.putText(" ");
          switch (n) {
            case "ordinary":
              this._html.putText(i);
              break;
            case "math":
              if (typeof e === "undefined") {
                throw "No math backend found. Please setup KaTeX or MathJax."
              } else if (e.name === "katex") {
                this._html.putHTML(e.driver.renderToString(i))
              } else if (e.name === "mathjax") {
                this._html.putText("$" + i + "$")
              } else {
                throw "Unknown math backend " + e
              }
              break;
            case "cond-symbol":
              this._html.beginSpan("ps-keyword").putText(i.toLowerCase()).endSpan();
              break;
            case "special":
              if (i === "\\\\") {
                this._html.putHTML("<br/>");
                break
              }
              var r = {
                "\\{": "{",
                "\\}": "}",
                "\\$": "$",
                "\\&": "&",
                "\\#": "#",
                "\\%": "%",
                "\\_": "_"
              };
              var o = r[i];
              this._html.putText(o);
              break;
            case "text-symbol":
              var s = {
                textbackslash: "\\"
              };
              var a = s[i];
              this._html.putText(a);
              break;
            case "quote-symbol":
              var l = {
                "`": "\u2018",
                "``": "\u201c",
                "'": "\u2019",
                "''": "\u201d"
              };
              var h = l[i];
              this._html.putText(h);
              break;
            case "call":
              this._html.beginSpan("ps-funcname").putText(i).endSpan();
              this._html.write("(");
              var p = t.children[0];
              this._renderCloseText(p, e);
              this._html.write(")");
              break;
            case "close-text":
              this._renderCloseText(t, e);
              break;
            case "font-dclr":
            case "sizing-dclr":
              this._textStyle.updateByCommand(i);
              this._html.beginSpan(null, this._textStyle.toCSS());
              var u = new B(this._nodes, this._textStyle);
              this._html.putHTML(u.renderToHTML(e));
              this._html.endSpan();
              break;
            case "font-cmd":
              var c = this._nodes[0];
              if (c.type !== "close-text")
                continue;
              var f = new A(this._textStyle.fontSize());
              f.updateByCommand(i);
              this._html.beginSpan(null, f.toCSS());
              var d = new B(c.children, f);
              this._html.putHTML(d.renderToHTML(e));
              this._html.endSpan();
              break;
            default:
              throw new ParseError("Unexpected ParseNode of type " + t.type)
          }
        }
        return this._html.toMarkup()
      }
        ;
      function _() {
        this._body = [];
        this._textBuf = []
      }
      _.prototype.beginDiv = function (e, t, n) {
        this._beginTag("div", e, t, n);
        this._body.push("\n");
        return this
      }
        ;
      _.prototype.endDiv = function () {
        this._endTag("div");
        this._body.push("\n");
        return this
      }
        ;
      _.prototype.beginP = function (e, t, n) {
        this._beginTag("p", e, t, n);
        this._body.push("\n");
        return this
      }
        ;
      _.prototype.endP = function () {
        this._flushText();
        this._endTag("p");
        this._body.push("\n");
        return this
      }
        ;
      _.prototype.beginSpan = function (e, t, n) {
        this._flushText();
        return this._beginTag("span", e, t, n)
      }
        ;
      _.prototype.endSpan = function () {
        this._flushText();
        return this._endTag("span")
      }
        ;
      _.prototype.putHTML = function (e) {
        this._flushText();
        this._body.push(e);
        return this
      }
        ;
      _.prototype.putText = function (e) {
        this._textBuf.push(e);
        return this
      }
        ;
      _.prototype.write = function (e) {
        this._body.push(e)
      }
        ;
      _.prototype.toMarkup = function () {
        this._flushText();
        var e = this._body.join("");
        return e.trim()
      }
        ;
      _.prototype.toDOM = function () {
        var e = this.toMarkup();
        var t = document.createElement("div");
        t.innerHTML = e;
        return t.firstChild
      }
        ;
      _.prototype._flushText = function () {
        if (this._textBuf.length === 0)
          return;
        var e = this._textBuf.join("");
        this._body.push(this._escapeHtml(e));
        this._textBuf = []
      }
        ;
      _.prototype._beginTag = function (e, t, n, i) {
        var r = "<" + e;
        if (t)
          r += ' class="' + t + '"';
        if (n) {
          var o;
          if (a.isString(n))
            o = n;
          else {
            o = "";
            for (var s in n) {
              attrVal = n[s];
              o += s + ":" + attrVal + ";"
            }
          }
          if (i)
            o += i;
          r += ' style="' + o + '"'
        }
        r += ">";
        this._body.push(r);
        return this
      }
        ;
      _.prototype._endTag = function (e) {
        this._body.push("</" + e + ">");
        return this
      }
        ;
      var i = {
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#39;",
        "/": "&#x2F;"
      };
      _.prototype._escapeHtml = function (e) {
        return String(e).replace(/[&<>"'/]/g, function (e) {
          return i[e]
        })
      }
        ;
      function r(e) {
        e = e || {};
        this.indentSize = e.indentSize ? this._parseEmVal(e.indentSize) : 1.2;
        this.commentDelimiter = e.commentDelimiter !== undefined ? e.commentDelimiter : " // ";
        this.lineNumberPunc = e.lineNumberPunc !== undefined ? e.lineNumberPunc : ":";
        this.lineNumber = e.lineNumber !== undefined ? e.lineNumber : false;
        this.noEnd = e.noEnd !== undefined ? e.noEnd : false;
        if (e.captionCount !== undefined)
          F.captionCount = e.captionCount;
        this.titlePrefix = e.titlePrefix !== undefined ? e.titlePrefix : "Algorithm"
      }
      r.prototype._parseEmVal = function (e) {
        e = e.trim();
        if (e.indexOf("em") !== e.length - 2)
          throw "option unit error; no `em` found";
        return Number(e.substring(0, e.length - 2))
      }
        ;
      function F(e, t) {
        this._root = e.parse();
        this._options = new r(t);
        this._openLine = false;
        this._blockLevel = 0;
        this._textLevel = -1;
        this._globalTextStyle = new A;
        this.backend = undefined;
        try {
          if (typeof katex === "undefined")
            katex = n("katex")
        } catch (e) { }
        try {
          if (typeof MathJax === "undefined")
            MathJax = n("mathjax")
        } catch (e) { }
        if (typeof katex !== "undefined") {
          this.backend = {
            name: "katex",
            driver: katex
          }
        } else if (typeof MathJax !== "undefined") {
          this.backend = {
            name: "mathjax",
            driver: MathJax
          }
        }
      }
      F.captionCount = 0;
      F.prototype.toMarkup = function () {
        var e = this._html = new _;
        this._buildTree(this._root);
        delete this._html;
        return e.toMarkup()
      }
        ;
      F.prototype.toDOM = function () {
        var e = this.toMarkup();
        var t = document.createElement("div");
        t.innerHTML = e;
        return t.firstChild
      }
        ;
      F.prototype._beginGroup = function (e, t, n) {
        this._closeLineIfAny();
        this._html.beginDiv("ps-" + e + (t ? " " + t : ""), n)
      }
        ;
      F.prototype._endGroup = function (e) {
        this._closeLineIfAny();
        this._html.endDiv()
      }
        ;
      F.prototype._beginBlock = function () {
        var e = this._options.lineNumber && this._blockLevel === 0 ? .6 : 0;
        var t = this._options.indentSize + e;
        this._beginGroup("block", null, {
          "margin-left": t + "em"
        });
        this._blockLevel++
      }
        ;
      F.prototype._endBlock = function () {
        this._closeLineIfAny();
        this._endGroup();
        this._blockLevel--
      }
        ;
      F.prototype._newLine = function () {
        this._closeLineIfAny();
        this._openLine = true;
        this._globalTextStyle.outerFontSize(1);
        var e = this._options.indentSize;
        if (this._blockLevel > 0) {
          this._numLOC++;
          this._html.beginP("ps-line ps-code", this._globalTextStyle.toCSS());
          if (this._options.lineNumber) {
            this._html.beginSpan("ps-linenum", {
              left: -((this._blockLevel - 1) * (e * 1.25)) + "em"
            }).putText(this._numLOC + this._options.lineNumberPunc).endSpan()
          }
        } else {
          this._html.beginP("ps-line", {
            "text-indent": -e + "em",
            "padding-left": e + "em"
          }, this._globalTextStyle.toCSS())
        }
      }
        ;
      F.prototype._closeLineIfAny = function () {
        if (!this._openLine)
          return;
        this._html.endP();
        this._openLine = false
      }
        ;
      F.prototype._typeKeyword = function (e) {
        this._html.beginSpan("ps-keyword").putText(e).endSpan()
      }
        ;
      F.prototype._typeFuncName = function (e) {
        this._html.beginSpan("ps-funcname").putText(e).endSpan()
      }
        ;
      F.prototype._typeText = function (e) {
        this._html.write(e)
      }
        ;
      F.prototype._buildTreeForAllChildren = function (e) {
        var t = e.children;
        for (var n = 0; n < t.length; n++)
          this._buildTree(t[n])
      }
        ;
      F.prototype._buildCommentsFromBlock = function (e) {
        var t = e.children;
        while (t.length > 0 && t[0].type === "comment") {
          var n = t.shift();
          this._buildTree(n)
        }
      }
        ;
      F.prototype._buildTree = function (e) {
        var t;
        var n;
        var i;
        switch (e.type) {
          case "root":
            this._beginGroup("root");
            this._buildTreeForAllChildren(e);
            this._endGroup();
            break;
          case "algorithm":
            var r;
            for (t = 0; t < e.children.length; t++) {
              n = e.children[t];
              if (n.type !== "caption")
                continue;
              r = n;
              F.captionCount++
            }
            if (r) {
              this._beginGroup("algorithm", "with-caption");
              this._buildTree(r)
            } else {
              this._beginGroup("algorithm")
            }
            for (t = 0; t < e.children.length; t++) {
              n = e.children[t];
              if (n.type === "caption")
                continue;
              this._buildTree(n)
            }
            this._endGroup();
            break;
          case "algorithmic":
            if (this._options.lineNumber) {
              this._beginGroup("algorithmic", "with-linenum");
              this._numLOC = 0
            } else {
              this._beginGroup("algorithmic")
            }
            this._buildTreeForAllChildren(e);
            this._endGroup();
            break;
          case "block":
            this._beginBlock();
            this._buildTreeForAllChildren(e);
            this._endBlock();
            break;
          case "function":
            var o = e.value.type.toLowerCase();
            var s = e.value.name;
            i = e.children[0];
            var a = e.children[1];
            this._newLine();
            this._typeKeyword(o + " ");
            this._typeFuncName(s);
            this._typeText("(");
            this._buildTree(i);
            this._typeText(")");
            this._buildCommentsFromBlock(a);
            this._buildTree(a);
            if (!this._options.noEnd) {
              this._newLine();
              this._typeKeyword("end " + o)
            }
            break;
          case "if":
            this._newLine();
            this._typeKeyword("if ");
            ifCond = e.children[0];
            this._buildTree(ifCond);
            this._typeKeyword(" then");
            var l = e.children[1];
            this._buildCommentsFromBlock(l);
            this._buildTree(l);
            var h = e.value.numElif;
            for (var p = 0; p < h; p++) {
              this._newLine();
              this._typeKeyword("else if ");
              var u = e.children[2 + 2 * p];
              this._buildTree(u);
              this._typeKeyword(" then");
              var c = e.children[2 + 2 * p + 1];
              this._buildCommentsFromBlock(c);
              this._buildTree(c)
            }
            var f = e.value.hasElse;
            if (f) {
              this._newLine();
              this._typeKeyword("else");
              var d = e.children[e.children.length - 1];
              this._buildCommentsFromBlock(d);
              this._buildTree(d)
            }
            if (!this._options.noEnd) {
              this._newLine();
              this._typeKeyword("end if")
            }
            break;
          case "loop":
            this._newLine();
            var _ = e.value;
            var m = {
              for: "for",
              forall: "for all",
              while: "while"
            };
            this._typeKeyword(m[_] + " ");
            var x = e.children[0];
            this._buildTree(x);
            this._typeKeyword(" do");
            var y = e.children[1];
            this._buildCommentsFromBlock(y);
            this._buildTree(y);
            if (!this._options.noEnd) {
              this._newLine();
              var v = _ === "while" ? "end while" : "end for";
              this._typeKeyword(v)
            }
            break;
          case "repeat":
            this._newLine();
            this._typeKeyword("repeat");
            var b = e.children[0];
            this._buildCommentsFromBlock(b);
            this._buildTree(b);
            this._newLine();
            this._typeKeyword("until ");
            var w = e.children[1];
            this._buildTree(w);
            break;
          case "upon":
            this._newLine();
            this._typeKeyword("upon ");
            uponCond = e.children[0];
            this._buildTree(uponCond);
            var g = e.children[1];
            this._buildCommentsFromBlock(g);
            this._buildTree(g);
            if (!this._options.noEnd) {
              this._newLine();
              this._typeKeyword("end upon")
            }
            break;
          case "command":
            var T = e.value;
            var k = {
              break: "break",
              continue: "continue"
            }[T];
            this._newLine();
            if (k)
              this._typeKeyword(k);
            break;
          case "caption":
            this._newLine();
            this._typeKeyword(this._options.titlePrefix + " " + F.captionCount + " ");
            i = e.children[0];
            this._buildTree(i);
            break;
          case "comment":
            i = e.children[0];
            this._html.beginSpan("ps-comment");
            this._html.putText(this._options.commentDelimiter);
            this._buildTree(i);
            this._html.endSpan();
            break;
          case "statement":
            var C = e.value;
            var S = {
              state: "",
              ensure: "Ensure: ",
              require: "Require: ",
              input: "Input: ",
              output: "Output: ",
              print: "print ",
              return: "return "
            }[C];
            this._newLine();
            if (S)
              this._typeKeyword(S);
            i = e.children[0];
            this._buildTree(i);
            break;
          case "open-text":
            var L = new B(e.children, this._globalTextStyle);
            this._html.putHTML(L.renderToHTML(this.backend));
            break;
          case "close-text":
            var E = this._globalTextStyle.fontSize();
            var M = new A(E);
            var z = new B(e.children, M);
            this._html.putHTML(z.renderToHTML(this.backend));
            break;
          default:
            throw new ParseError("Unexpected ParseNode of type " + e.type)
        }
      }
        ;
      e.exports = F
    }
      , {
      "./utils": 6,
      katex: undefined,
      mathjax: undefined
    }],
    6: [function (e, t, n) {
      function i(e) {
        return typeof e === "string" || e instanceof String
      }
      function r(e) {
        return typeof e === "object" && e instanceof Object
      }
      function o(e) {
        if (!r(e))
          return e + "";
        var t = [];
        for (var n in e)
          t.push(n + ": " + o(e[n]));
        return t.join(", ")
      }
      t.exports = {
        isString: i,
        isObject: r,
        toString: o
      }
    }
      , {}]
  }, {}, [1])(1)
});
