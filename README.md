![deusXmachina Logo](static/logo.png)

> Industrial dev tools — built by engineers who actually touch robots ⚙️🤖  
> No LLMs were seriously hurt during the making of these tools — though OpenAI *did* have to raise a few Bs 💸.

---

# Chapter 1: The Who, the What, and the Why

We're currently a team of two:
- [@pungys97](https://github.com/pungys97)
- [@FilipKubis](https://github.com/FilipKubis)

Both software engineers with a shared origin story in **mechanical engineering and robotics**.  
We started with robots, drifted into software — because, well, software is fun… but now we're fiddling again with robots, because they are **funnerer** (until they take over).

We're optimistic by default — just mildly annoyed by the current mood in the European industry scene.  
There's a *ton* of cool stuff to be built, and this project is our first step toward building the tools — and the feedback loops — to make that happen.

---

## Our Mission

We're building **developer tools for industrial engineers**.  
Our first project: an **energy optimization tool for robots** (details below).

Here's the catch: industry desperately needs good software engineers…  
but anyone who can code usually jumps ship to software — because of the 💰 (fair enough).  
We've seen this pattern again and again among our robotics friends at uni.

---

## The European Industry Rant™

Everyone says industry in Europe is dying — and honestly, it *feels* that way sometimes.  
Which sucks, because this continent **invented** industry.

The struggle?  
- Expensive inputs (energy — “green” power, but sadly not *Hulk* green)  
- Costly labor  
- Shifting work ethics (we actually *like* the idea of a 4-day work week — but only when productivity keeps up)  
- Declining population  

Put it all together and, well… we're kinda cooked.

The way out?  
- Either **innovate like crazy**  
- Or **fix the demographics** (more *shabonking* — though that part’s not going on GitHub 😅)

---

# Chapter 2: ⚡ Energy Optimization for Industrial Robots

The first concept we're throwing out into the world: **energy optimization for industrial robots**.

The idea is pretty simple — in a typical production line, a few **bottleneck cells** determine the overall takt time.  
Meanwhile, the rest of the robots just chill — wasting energy.

So instead of letting them idle at full power, why not *strategically slow down* some movements?  
You’ll save a surprising amount of **energy**, with **zero impact** on throughput.

---

## 🧩 Other Perks
- Less **wear & tear** — this adds up fast  
- Lower **CO₂ footprint** (if that’s your thing) 💅  

---

## 🛠️ The Solution

The concept is simple:  
> **Slow down the right robots at the right time.**

The challenge? Doing this **at scale**, and figuring out **which movements** to slow down — without breaking takt time.

That’s where we come in.  
We’re open-sourcing a **lightweight proof-of-concept** to test this idea in the wild.

We’re releasing a **plugin for Siemens Process Simulate** — a heuristic-driven prototype built on real-world motion data.  
It’s a “good-enough” starting point: hacky, simple, and functional.

Try it. Break it. Tell us what works (and what doesn’t).  
There’s a lot of room to make this smarter, faster, and more tailored to your setup — but we’ll need your feedback to get there.

---

## 🚀 How to Use the Plugin

1. Install the plugin in **Siemens Process Simulate**  
2. Run a simulation of your **digital twins** (or as we call them — *digital twinziess™*)  
3. Apply the **Energy Optimization** plugin and review suggested slowdowns  
4. Compare **energy savings** vs. baseline  
5. **Profitzzz** — *“corporate just orgasmed”*

---

## Why Open Source?

Because good ideas deserve daylight.  
We want **feedback, hacks, forks** — whatever helps push industrial tech forward.  
We're not precious about it. Just build cool stuff.  

---

### deusXmachina  
Built with optimism. Fueled by frustration.  
Robots first, profits second.