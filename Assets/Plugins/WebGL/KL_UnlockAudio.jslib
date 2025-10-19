mergeInto(LibraryManager.library, {
    KL_UnlockAudio: function () {
        if (typeof window === 'undefined') return;
        try {
            var AC = window.AudioContext || window.webkitAudioContext;
            if (!AC) return;

            if (!window.__kl_ctx) window.__kl_ctx = new AC();
            var ctx = window.__kl_ctx;

            if (ctx.state === 'suspended') { ctx.resume(); }

            var buf = ctx.createBuffer(1, 1, 22050);
            var src = ctx.createBufferSource();
            src.buffer = buf;
            src.connect(ctx.destination);
            if (src.start) src.start(0);
        } catch (e) { /* no-op */ }
    }
});
