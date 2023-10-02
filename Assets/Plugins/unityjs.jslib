mergeInto(LibraryManager.library, {
  createHost: function(token, room_name) {
    window.createHost(UTF8ToString(token), UTF8ToString(room_name));
  },

  createClient: function(token, room_name) {
    window.createClient(UTF8ToString(token), UTF8ToString(room_name));
  },

  writeStream: function(message) {
    window.writeStream(UTF8ToString(message));
  },

  reCreateToken: function(token) {
    window.recreateToken(UTF8ToString(token));
  },

  changeJoin: function(mode) {
    window.changeJoin(mode);
  },

  getMembers: function() {
    return window.room.members.length;
  },

  closeRoom: function() {
    window.closeRoom();
  },

  initPlayer: function() {
    window.initPlayer();
  },

  createSong: function(songUrl) {
    window.player.createFromSongUrl(UTF8ToString(songUrl));
  },

  createSongConfig: function(songUrl, beatId, chordId, repetitiveSegmentId, lyricId, lyricDiffId) {
    window.player.createFromSongUrl(UTF8ToString(songUrl), {
      video: {
        beatId: beatId,
        chordId: chordId,
        repetitiveSegmentId: repetitiveSegmentId,
        lyricId: lyricId,
        lyricDiffId: lyricDiffId
      }
    });
  },

  play: function() {
    window.player.requestPlay();
  },

  stop: function() {
    window.player.requestStop();
  },

  pause: function() {
    window.player.requestPause();
  },

  position: function() {
    return window.player.timer.position;
  },

  volume: function() {
    return window.volume;
  },

  skyway: function() {
    return window.skyway_load;
  },

  finalChorus: function() {
    return window.player.timer.position < window.final_chorus;
  },

  findPhrase: function() {
    var pos = window.player.timer.position;
    var phrase = window.player.video.findPhrase(pos);
    var returnStr;
    if (phrase) {
      returnStr = window.replaceText(phrase.text);
    }else  {
      returnStr = "";
    }

    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  findPhraseCount: function() {
    var count = 0;
    var phrase = window.player.video.findPhrase(window.player.timer.position)
    if (phrase) {
      phrase.children.forEach(word => {
        word.children.forEach(ch => {
          count ++;
        });
      }); 
    }
  
    return count;
  },

  findWord: function() {
    var pos = window.player.timer.position;
    var word = window.player.video.findWord(pos);
    var returnStr;
    if (word) {
      returnStr = window.replaceText(word.text);
    }else  {
      returnStr = "";
    }

    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  findChar: function() {
    var pos = window.player.timer.position;
    var char = window.player.video.findChar(pos);
    var returnStr;
    if (char) {
      returnStr = window.replaceText(char.text);
    }else  {
      returnStr = "";
    }

    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  findBeat: function() {
    var pos = window.player.timer.position;
    var beat = window.player.findBeat(pos);
    if (beat) {
      return beat.index;
    } else {
      return -1;
    }
  },

  findChord: function() {
    var pos = window.player.timer.position;
    var chord = window.player.findChord(pos);
    var returnStr = "";
    if (chord) {
      returnStr = chord.name;
    }
    
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  findChorus: function() {
    var pos = window.player.timer.position;
    var chorus = window.player.findChorus(pos);
    if (chorus) {
      return chorus.index;
    } else {
      return -1;
    }
  },

  getUrl: function() {
    let url = encodeURIComponent(location.href);
    var bufferSize = lengthBytesUTF8(url) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(url, buffer, bufferSize);
    return buffer;
  },

  urlOpen: function(url) {
    var Url_str = UTF8ToString(url);
    window.open(Url_str);
  },
});


