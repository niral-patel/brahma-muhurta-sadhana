# Brahma Muhurta Sadhana

A guided early-morning routine (around 3:30 AM to 4:30 AM) that walks you step by step through
exercise, pranayama, meditation and puja, with how-to instructions for every step, a wake-up alarm,
and a Panchang with muhurat times for Dallas, TX.

**Open the app:** https://niral-patel.github.io/brahma-muhurta-sadhana/

It is a small web app with no account and no audio files. It runs the same on Windows, Mac and iPhone,
and works offline after the first visit.

This is an extra self-help project, separate from my ML/AI portfolio.

## Install it as an app

| Device | How |
|---|---|
| iPhone | Open the link in **Safari** → **Share** → **Add to Home Screen**. It opens full screen from its icon. |
| Windows / Mac (Chrome or Edge) | Open the link → **Install** icon in the address bar, or menu → **Save and share** → **Install**. |
| Mac (Safari) | Open the link → **File** → **Add to Dock**. |

## The four tabs

| Tab | What it shows |
|---|---|
| **Today** | The current step with a countdown, how to do it, today's Panchang summary and the alarm |
| **Routine** | Every step: switch it on or off, set minutes (or rounds), reorder, jump to it |
| **Panchang** | Tithi, nakshatra, yoga, karana, sunrise, sunset, muhurat and choghadiya for Dallas, any day |
| **Guide** | Suggestions for keeping the routine, how to do each step, and the weekly meditation plan |

## The default routine (60 minutes)

| Time | Step | Guidance on screen |
|---|---|---|
| 3:30 | Wake up and freshen (4 min) | Countdown |
| 3:34 | Walk (4 min) | Countdown |
| 3:38 | Surya Namaskar (4 rounds) | 12 postures listed; tap **Round done** after each round |
| 3:44 | Hanuman dand (3 min) | 40 s work / 20 s rest |
| 3:47 | Push-ups (3 min) | 40 s work / 20 s rest |
| 3:50 | Plank (2 min) | 45 s hold / 15 s rest |
| 3:52 | Cold shower (4 min) | Countdown |
| 3:56 | Kapalbhati (5 min) | Stroke counter, one per second, 45 s on / 15 s rest |
| 4:01 | Anulom Vilom (5 min) | Left/right cues; the circle grows on inhale (4) and shrinks on exhale (8) |
| 4:06 | Bhramari (3 min) | Inhale 4, hum 8 |
| 4:09 | Ujjayi (3 min) | Inhale 5, exhale 5 |
| 4:12 | Meditation (10 min) | Opening bell, a drone in the pitch of the day, soft bell every 5 minutes, closing bell |
| 4:22 | Puja (8 min) | Countdown |

Bhastrika, Sheetali, Shavasana and Japa are included but switched off. Tick them in **Routine** to add them.
Surya Namaskar is counted in rounds, not minutes; change the number of rounds in **Routine**.

## Sound

Only two things make sound: the **wake-up alarm** and the **meditation** (bells and drone).
Every other step is silent and guided on screen. There is no voice.

## Meditation through the week

| Day | Focus | Technique | Mantra |
|---|---|---|---|
| Mon | Muladhara, base of the spine | Body scan | Lam |
| Tue | Svadhisthana, below the navel | Breath counting | Vam |
| Wed | Manipura, navel | Flame visualisation | Ram |
| Thu | Anahata, centre of the chest | Maitri (loving-kindness) | Yam |
| Fri | Vishuddha, throat | Om chanting | Ham |
| Sat | Ajna, between the eyebrows | Dharana (one-point focus) | Om |
| Sun | Sahasrara, crown | Open awareness | silent Om |

## Panchang and muhurat

Calculated on the device for **Dallas, TX** (32.78° N, 96.80° W) in Central time, using the
[astronomy-engine](https://github.com/cosinekitty/astronomy) library and the same method as my
Morning Briefing app: Lahiri ayanamsa, amanta months, day counted from sunrise to sunrise.

It shows the month and Vikram Samvat, paksha, tithi, nakshatra, yoga, karana, sunrise and sunset,
Brahma muhurta, Abhijit muhurta, Rahu kaal, Yamaganda, Gulika kaal and the day and night choghadiya.
Times can differ by a minute or two from printed panchangs. Festivals are not listed.

## The alarm, and its limit

A web page can only ring while it is open with the screen on. So:

- On a laptop or desktop: set the alarm in the app, leave the page open and the machine plugged in.
- On iPhone: use the Clock app as the real wake-up alarm, then open this app and press **Start**.

## Care

Skip Kapalbhati and Bhastrika if you are pregnant or have high blood pressure, heart disease, a hernia
or recent abdominal surgery. Ease into cold showers if you have a heart condition. Stop any practice
that makes you dizzy and breathe normally. This app is a timer and guide, not medical advice.

## Files

| File | Purpose |
|---|---|
| `index.html` | The whole app: layout, styles and logic |
| `astronomy.js` | astronomy-engine 2.1.19 (MIT), sun and moon positions for the Panchang |
| `sw.js` | Service worker that caches the app for offline use |
| `manifest.webmanifest`, `icon-*.png`, `apple-touch-icon.png` | Install-as-app details and icons |
