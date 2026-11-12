// Offline support: the app files are cached, answered from cache at once and refreshed in the background.
const VERSION = "bms-v5";
const SHELL = ["./", "index.html", "astronomy.js", "panchang-data.js", "cities.json", "manifest.webmanifest", "apple-touch-icon.png", "icon-192.png", "icon-512.png"];

self.addEventListener("install", e => {
  e.waitUntil(caches.open(VERSION).then(c => c.addAll(SHELL)).then(() => self.skipWaiting()));
});

self.addEventListener("activate", e => {
  e.waitUntil(caches.keys()
    .then(keys => Promise.all(keys.filter(k => k !== VERSION).map(k => caches.delete(k))))
    .then(() => self.clients.claim()));
});

self.addEventListener("fetch", e => {
  const url = new URL(e.request.url);
  if (e.request.method !== "GET" || url.origin !== location.origin) return;
  e.respondWith(caches.open(VERSION).then(async c => {
    const cached = await c.match(e.request);
    const fresh = fetch(e.request).then(res => {
      if (res.ok) c.put(e.request, res.clone());
      return res;
    }).catch(() => cached);
    return cached || fresh;
  }));
});
