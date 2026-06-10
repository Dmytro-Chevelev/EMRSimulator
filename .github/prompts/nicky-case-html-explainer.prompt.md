---
agent: agent
description: Generate a single-file, zero-dependency interactive HTML explainer on any topic, in the style of Nicky Case.
---

Build a **single-file, zero-dependency interactive HTML explainer** about **[insert topic here]**.

[Insert any additional topic-specific instructions here — e.g., key concepts to cover, specific interactions, target audience, length, etc.]

---

## Style & Tone (Nicky Case)

Write in the voice and style of Nicky Case's explorable explanations (e.g., *The Evolution of Trust*, *Parable of the Polygons*):

- **Conversational and direct.** Talk to the reader as "you." Use short sentences. Say the weird thing out loud.
- **Intellectually honest.** Acknowledge nuance, tradeoffs, and uncertainty. Don't oversimplify into propaganda.
- **Playful but substantive.** Humor is welcome, but every joke should earn its place. The content is the point.
- **Show, don't just tell.** If something can be demonstrated interactively, make it interactive.
- **Progressive disclosure.** Introduce one idea at a time. Let the reader act before moving to the next idea.

---

## Structure

Organize the explainer as a vertical, scroll-driven narrative with interactive sections embedded inline. Follow this general arc:

1. **Hook** — Open with a concrete, relatable scenario or a surprising question. No preamble.
2. **Core concept(s)** — Introduce ideas one at a time, each paired with an interaction or illustration.
3. **Twist or complication** — Show where the simple model breaks down or gets interesting.
4. **Implication** — What does this mean? What should the reader do, think, or notice differently?
5. **Closing thought** — Brief. Memorable. Not preachy.

---

## Interactions

Include at least **2–3 meaningful interactive elements**. Good options include:

- **Sliders** that change a value and immediately update a simulation or diagram.
- **Clickable toggles** that flip a state and show consequences.
- **Step-through simulations** — a "Next Step" button that advances a process one tick at a time.
- **Simple sandboxes** — let the reader experiment with parameters and observe outcomes.
- **Reactive text** — prose that updates inline as the reader changes inputs.

Each interaction should be **tightly coupled to a concept**. Do not add interactions for decoration.

---

## Visual Design

- **Minimal color palette.** Use at most 3–4 colors. Default to black, white, and one accent color. Add a second accent only if semantically necessary (e.g., two opposing forces).
- **No external fonts, icons, libraries, or CDN links.** Everything must be self-contained in a single `.html` file.
- **Use SVG or Canvas for diagrams.** Prefer SVG for static/semi-static shapes; Canvas for animations or dense simulations.
- **Layout:** Single-column, centered, max-width ~680px. Generous whitespace. Let the content breathe.
- **Typography:** System font stack. Body ~18px, comfortable line-height (~1.6). Headers only when necessary.
- **Interactive elements** should feel native and obvious — no mystery meat controls.

---

## Technical Requirements

- **Single `.html` file.** All CSS and JavaScript must be inline (`<style>` and `<script>` tags).
- **Zero external dependencies.** No frameworks, no CDN assets, no `import` statements.
- **No build step.** The file must open and work correctly when double-clicked from the filesystem.
- **Vanilla JS only.** ES6+ is fine. Use `const`, `let`, arrow functions, `querySelectorAll`, etc.
- **Responsive.** Must render reasonably on both desktop and mobile viewports.
- **Accessible.** Interactive elements must have labels. Use semantic HTML. Ensure sufficient color contrast.

---

## Output

Produce the complete, final `.html` file. Do not produce a skeleton or placeholder — the file should be fully functional and contain real content, real interactions, and real prose about the topic.

Do not include any explanation outside the HTML file itself. The file is the deliverable.
